using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFigureController : MonoBehaviour
{
    public GameObject marker;
    public float moveSpeed;
    private bool isWalking = false;
    private Vector3 destination;
    private bool markerEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            if(!markerEnabled) {
                markerEnabled = true;
                marker.SetActive(true);
            }
            GetComponentInParent<ShadowPuzzleManager>().ReleasePlayer();
        }
    }

    public void SetDestination(Vector3 dest) {
        destination = dest;
        isWalking = true;
        GetComponent<Animator>().SetBool("isWalking", true);
    }

    
}
