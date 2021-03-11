using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public Transform destination;
    public bool canScatter = false;
    public float minDistance = 1.0f;
    public float moveSpeed = 2.0f;
    public float scatterSpeed = 0.2f;
    private Vector3 screenPoint;
    private Vector3 offset;

    private bool isNear = false;
    private bool isPlaced = false;
    private bool isScattering = true;
    private bool hasPlaced = false;
    private Vector2 scatterDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scatter();
        AutoMove();
    }

    private void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - 
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag() {
        Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint) + offset;
        transform.position = cursorPosition;
        isScattering = false;
    }

    private void OnMouseUp() {
        if(Vector3.Distance(transform.position, destination.position) < minDistance){
            isNear = true;
        }
    }

    private void Scatter() {
        if(!isScattering) {
            return;
        }
        transform.position += RotateVector(scatterDir, Random.Range(-45,45)) * Time.deltaTime * 
            scatterSpeed * 1f/Mathf.Pow((1f + Vector3.Distance(transform.position, destination.position)),2);
    }

    private void AutoMove() {
        if(isNear) {
            Vector3 distance = destination.position - transform.position;
            transform.position += distance * moveSpeed * Time.deltaTime * 
            Mathf.Pow(minDistance / distance.magnitude, 2);
            if(Vector3.Distance(destination.position, transform.position) <= 0.05f) {
                isNear = false;
                if(hasPlaced || !canScatter) {
                    isPlaced = true;
                    transform.position = destination.position;
                }
                else {
                    hasPlaced = true;
                    isScattering = true;
                    scatterDir = (destination.position - transform.position).normalized;
                    print(scatterDir);
                }
            }
        }
    }

    public Vector3 RotateVector(Vector2 v, float angle){
        float radian = angle*Mathf.Deg2Rad;
        float _x = v.x*Mathf.Cos(radian) - v.y*Mathf.Sin(radian);
        float _y = v.x*Mathf.Sin(radian) + v.y*Mathf.Cos(radian);
        return new Vector3(_x, _y, 0);
    }
    

    public bool CheckPlaced() {
        return isPlaced;
    }
}
