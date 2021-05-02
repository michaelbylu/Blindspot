using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{ 
    public AnimatorOverrideController[] animatorOverrideControllers;
    public float moveSpeed = 2.0f;
    public Transform chair;
    public int sitSortingOrder = 0;
    private Vector3 target;
    private bool isMoving = false;
    private bool isSeated = false;
    private bool canMove = true;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Animator animator;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        target = transform.position;
        animator = GetComponentInChildren<Animator>();
        if(animatorOverrideControllers.Length > 0) {
            animator.runtimeAnimatorController = animatorOverrideControllers[0];
        }
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
        GeneratePath();
    }

    private void Move() {
        if(!isMoving || !canMove) {
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
        if(Vector3.Distance(transform.position, chair.position) <= 0.9f && !isSeated) {
            transform.position = chair.position;
            isMoving = false;
            isSeated = true;
            canMove = false;
            if(sitSortingOrder != 0) {
                GetComponentInChildren<SpriteRenderer>().sortingOrder = sitSortingOrder;
            }
            else {
                GetComponentInChildren<SpriteRenderer>().sortingOrder = -2;
            }
            animator.SetBool("isSit", true);
            foreach(FadeInOut f in chair.GetComponentsInChildren<FadeInOut>()) {
                if(f.blinkAtStart) {
                    f.gameObject.SetActive(false);
                }
            }
            if(chair.GetComponentInChildren<ChairController>() != null) {
                chair.GetComponentInChildren<ChairController>().enabled = false;
            }
            if(GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>() != null) {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
            }
            else if(GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>() != null) {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
            }
            else if(GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter3Manager>() != null) {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter3Manager>().ChangeStage();
            }
            animator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void GeneratePath() {
        if(animator.GetBool("isSit") || !canMove) {
            return;
        }
        if(Input.GetMouseButtonUp(0)) {
            target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            target.z = 0f;
            seeker.StartPath(transform.position, target, OnPathComplete);
            isMoving = true;
            currentWaypoint = 0;
            animator.SetBool("isWalking", true);
        }
    }

    public void Release() {
        StartCoroutine(HoldForRelease());
    }

    public void SwitchWalkAnimation(int index) {
        if(animatorOverrideControllers.Length == 0) {
            return;
        }
        animator.runtimeAnimatorController = animatorOverrideControllers[index];
        if(index >= 1) {
            isSeated = false;
            chair.GetComponentInChildren<FadeInOut>(true).gameObject.SetActive(true);
            if(chair.GetComponentInChildren<ChairController>(true) != null) {
                chair.GetComponentInChildren<ChairController>(true).enabled = true;
            }
        }
    }

    public void FreezeMove() {
        animator.SetTrigger("CheckFood");
        animator.SetBool("isWalking", false);
        animator.transform.localScale = new Vector3(1f, 1f, 1f);
        canMove = false;
        print("Freeze triggered");
    }

    IEnumerator HoldForRelease() {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isSit", false);
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Release");
        canMove = true;
    }
}
