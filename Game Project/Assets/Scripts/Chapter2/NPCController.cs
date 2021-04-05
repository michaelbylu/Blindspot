using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCController : MonoBehaviour
{
    public GameObject food;
    public Transform counter;
    public Transform chair;
    public float moveSpeed = 2f;
    private Animator animator;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool hasFood = false;
    [SerializeField]
    private int currentStage = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();
        if(currentStage == -1) {
            MoveTo(chair.position);
            animator.GetComponent<SpriteRenderer>().sortingOrder = -6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
        Move();
    }

    private void Move() {
        if(path == null || !isMoving) {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count) {
            isMoving = false;
            animator.SetBool("isWalking", false);
            animator.GetComponent<SpriteRenderer>().sortingOrder += 3;
            if(currentStage == 1) {
                food.SetActive(true);
            }
            if(currentStage == -1) {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
            }
            return;
        }
        if(path.vectorPath[currentWaypoint].x > gameObject.transform.position.x || 
            Mathf.Abs(path.vectorPath[currentWaypoint].x - gameObject.transform.position.x) <= 0.001f) {
            animator.transform.localScale = new Vector3(-1f,1f,1f);
        }
        else{
            animator.transform.localScale = new Vector3(1f,1f,1f);
        }
        gameObject.transform.position += moveSpeed * Time.deltaTime * 
            Vector3.Normalize(path.vectorPath[currentWaypoint] - gameObject.transform.position);
        if(Vector3.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) <= 0.2f) {
            gameObject.transform.position = path.vectorPath[currentWaypoint];
            currentWaypoint++;
        }
    }

    private void UpdateDistance() {
        animator.SetFloat("CounterDis", Vector3.Distance(transform.position, counter.position));
        animator.SetFloat("ChairDis", Vector3.Distance(transform.position, chair.position));
        if(Vector3.Distance(transform.position, chair.position) < 0.1f) {
            animator.transform.localScale = new Vector3(1f,1f,1f);
        }
    }

    public void GeneratePath(int index, int order) {
        if(index == 0) {
            animator.GetComponent<SpriteRenderer>().sortingOrder = order;
            seeker.StartPath(transform.position, counter.position, OnPathComplete);
            isMoving = true;
            currentWaypoint = 0;
            animator.SetBool("isWalking", true);
        }
        if(index == 1) {
            animator.GetComponent<SpriteRenderer>().sortingOrder = order;
            seeker.StartPath(transform.position, chair.position, OnPathComplete);
            isMoving = true;
            currentWaypoint = 0;
            animator.SetBool("hasFood", true);
        }
        currentStage = index;
    }

    private void OnPathComplete(Path p) {
        if(!p.error) {
            path = p;
            for(int i = 0; i < path.vectorPath.Count; i++) {
                Debug.Log(path.vectorPath[i]);
            }
        }
    }

    public void MoveTo(Vector3 dest) {
        seeker.StartPath(transform.position, dest, OnPathComplete);
        isMoving = true;
        currentWaypoint = 0;
        animator.SetBool("isWalking", true);
    }

    public void StartGrabFood(int order) {
        StartCoroutine(GrabFood(order));
    }

    IEnumerator GrabFood(int order) {
        GeneratePath(0, order);
        yield return new WaitForSeconds(8f);
        GeneratePath(1, order);
        yield return new WaitForSeconds(5f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
    }
}
