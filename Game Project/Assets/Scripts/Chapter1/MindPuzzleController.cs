using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPuzzleController : MonoBehaviour
{
    public Transform destination;
    public float minDistance = 1.0f;
    public float moveSpeed = 2.0f;
    public float dragSpeed = 3.0f;
    public float blockSpeed = 0.2f;
    public float[] xRange;
    public float[] yRange;
    private Vector3 nextPath;
    private Vector3 screenPoint;
    private Vector3 offset;

    private bool isNear = false;
    private bool isDragged = false;
    private bool isPlaced = false;

    private List<string> blockers;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        if(isNear) {
            Vector3 distance = destination.position - transform.position;
            transform.position += distance * moveSpeed * Time.deltaTime * 
            Mathf.Pow(minDistance / distance.magnitude, 2);
            if(Vector3.Distance(destination.position, transform.position) <= 0.01f) {
                transform.position = destination.position;
                isNear = false;
                isPlaced = true;
                GetComponentInParent<MindPuzzleManager>().PuzzlePlaced();
                audioSource.Play();
            }
        }
    }

    private void OnEnable() {
        nextPath = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0f);
        blockers = new List<string>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Float() {
        if(isDragged || isNear || isPlaced) {
            return;
        }
        transform.position += (nextPath - transform.position).normalized * moveSpeed * Time.deltaTime;
        if(Vector3.Distance(transform.position, nextPath) <= 0.1f) {
            nextPath = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0f);
        }
    }

    private void OnMouseDown() {
        if(isNear || isPlaced) {
            return;
        }
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - 
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        isDragged = true;
    }

    private void OnMouseDrag() {
        if(isNear || isPlaced) {
            return;
        }
        Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint) + offset;
        if(Vector3.Distance(cursorPosition, transform.position) <= Time.deltaTime * dragSpeed && blockers.Count == 0) {
            transform.position = cursorPosition;
            return;
        }
        transform.position += (cursorPosition - transform.position).normalized * Time.deltaTime *
            (blockers.Count == 0? dragSpeed : blockSpeed);
    }

    private void OnMouseUp() {
        if(isNear || isPlaced) {
            return;
        }
        isDragged = false;
        if(Vector3.Distance(transform.position, destination.position) < minDistance){
            isNear = true;
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    public bool CheckPlaced() {
        return isPlaced;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.name.StartsWith("text")) {
            if(!blockers.Contains(other.name)){
                blockers.Add(other.name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name.StartsWith("text")) {
            if(!blockers.Contains(other.name)){
                blockers.Add(other.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.name.StartsWith("text")) {
            if(blockers.Contains(other.name)){
                blockers.Remove(other.name);
            }
        }
    }
}
