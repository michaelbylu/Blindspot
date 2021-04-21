using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredFigureController : MonoBehaviour
{
    public float moveSpeed;
    private bool isWalking;
    private bool isMovable;
    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isMovable) {
            GetMouseDest();
        }
        Walk();
    }

    private void Walk() {
        if(!isWalking) {
            return;
        }
        Vector3 dir = (destination - transform.position).normalized;
        if(dir.x * transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        transform.position += Time.deltaTime * moveSpeed * dir;
        if(Vector3.Distance(transform.position, destination) < 0.3f) {
            GetComponent<Animator>().SetBool("isWalking", false);
            isWalking = false;
            transform.position = destination;
        }
    }

    private void GetMouseDest() {
        Vector3 cursorPos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination = new Vector3(cursorPos.x, transform.position.y, transform.position.z);
        isWalking = true;
        GetComponent<Animator>().SetBool("isWalking", true);
    }

    public void SetMovable(bool flag) {
        isMovable = flag;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isMovable) {
            GetComponentInParent<ShadowPuzzleManager>().FreezePlayer();
            GetComponent<Animator>().SetBool("isWalking", false);
            isWalking = false;
        }
    }
}
