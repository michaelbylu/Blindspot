using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxScale;

    public float speed;

    public float pow;

    private bool scaleUp = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scaleUp) {
            Vector3 newScale = transform.localScale + Vector3.one * speed * Time.deltaTime;
            if(newScale.x >= maxScale){
                transform.localScale = Vector3.one * maxScale;
                scaleUp = false;
                return;
            }
            transform.localScale = newScale;
        }
        else{
            Vector3 newScale = transform.localScale - Vector3.one * speed * Time.deltaTime * 
                Mathf.Pow(maxScale / transform.localScale.x, pow);
            if(newScale.x <= 0){
                Destroy(gameObject);
            }
            transform.localScale = newScale;
        }
    }
}
