using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{ 
    public float moveSpeed = 2.0f;
    public Transform chair;
    private Vector3 target;
    private bool isMoving = false;
    private bool isSeated = false;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Animator animator;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        target = transform.position;
        animator = GetComponentInChildren<Animator>();
    }

    private void OnPathComplete(Path p) {
        if(!p.error) {
            path = p;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetMouseButtonUp(0) && !isSeated) {
            target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            target.z = 0f;
            seeker.StartPath(transform.position, target, OnPathComplete);
            isMoving = true;
            currentWaypoint = 0;
            animator.SetBool("isWalking", true);
        }
    }

    private void Move() {
        if(!isMoving) {
            GetComponent<AudioSource>().Stop();
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count) {
            isMoving = false;
            animator.SetBool("isWalking", false);
            return;
        }
        if(path.vectorPath[currentWaypoint].x > gameObject.transform.position.x) {
            animator.transform.localScale = new Vector3(1f,1f,1f);
        }
        else{
            animator.transform.localScale = new Vector3(-1f,1f,1f);
        }
        if(isMoving && !GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Play();
        }
        gameObject.transform.position += moveSpeed * Time.deltaTime * 
            Vector3.Normalize(path.vectorPath[currentWaypoint] - gameObject.transform.position);
        if(Vector3.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) <= 0.2f) {
            gameObject.transform.position = path.vectorPath[currentWaypoint];
            currentWaypoint++;
        }
        if(Vector3.Distance(transform.position, chair.position) <= 0.1f) {
            transform.position = chair.position;
            isMoving = false;
            isSeated = true;
            chair.GetComponentInChildren<FadeInOut>().gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
            animator.transform.localScale = new Vector3(1f,1f,1f);
            animator.SetBool("isSit", true);
        }
    }
}
