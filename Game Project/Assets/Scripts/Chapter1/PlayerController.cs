using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{ 
    public float moveSpeed = 2.0f;
    private Vector3 target;
    private bool isMoving = false;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        target = transform.position;
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
        if(Input.GetMouseButtonUp(0)) {
            target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            target.z = 0f;
            seeker.StartPath(transform.position, target, OnPathComplete);
            isMoving = true;
            currentWaypoint = 0;
        }
    }

    private void Move() {
        if(!isMoving) {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count) {
            isMoving = false;
            return;
        }
        gameObject.transform.position += moveSpeed * Time.deltaTime * 
            Vector3.Normalize(path.vectorPath[currentWaypoint] - gameObject.transform.position);
        if(Vector3.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]) <= 0.1f) {
            gameObject.transform.position = path.vectorPath[currentWaypoint];
            currentWaypoint++;
        }
    }
}
