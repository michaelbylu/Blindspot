using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPuzzleController : MonoBehaviour
{
    public Transform destination;
    public float minDistance = 1.0f;
    public float moveSpeed = 2.0f;
    public float[] xRange;
    public float[] yRange;
    private Vector3 nextPath;
    private Vector3 screenPoint;
    private Vector3 offset;

    private bool isNear = false;
    private bool isDragged = false;
    private bool isPlaced = false;
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
            }
        }
    }

    private void OnEnable() {
        nextPath = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0f);
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
    }

    private void OnMouseDrag() {
        if(isNear || isPlaced) {
            return;
        }
        isDragged = true;
        Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    private void OnMouseUp() {
        if(isNear || isPlaced) {
            return;
        }
        isDragged = false;
        if(Vector3.Distance(transform.position, destination.position) < minDistance){
            isNear = true;
        }
    }

    public bool CheckPlaced() {
        return isPlaced;
    }
}
