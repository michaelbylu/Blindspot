using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float[] xRange;
    public float[] yRange;
    private Vector3 nextPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Float();
    }

    private void OnEnable() {
        nextPath = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0f);
    }

    private void Float() {
        transform.position += (nextPath - transform.position).normalized * moveSpeed * Time.deltaTime;
        if(Vector3.Distance(transform.position, nextPath) <= 0.1f) {
            nextPath = new Vector3(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]), 0f);
        }
    }
}
