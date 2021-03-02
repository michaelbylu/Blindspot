using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float moveSpeed = 2.0f;
    private Vector3 screenPoint;
    private bool isMoving = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.P)) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().JumpTo(0);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnMouseDown() {
        screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0f;
        isMoving = true;
    }

    private void Move() {
        if(!isMoving) {
            return;
        }
        gameObject.transform.position += moveSpeed * Time.deltaTime * 
            Vector3.Normalize(screenPoint - gameObject.transform.position);
        if(Vector3.Distance(gameObject.transform.position, screenPoint) <= 0.1f) {
            gameObject.transform.position = screenPoint;
            isMoving = false;
        }
    }
}
