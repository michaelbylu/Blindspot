using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    public float thickness;
    public float threshold;
    private Vector3 mousePos;
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mousePos.z = 0f;
        if(Vector3.Distance(mousePos, transform.position) <= threshold) {
            material.SetFloat("_Thickness", thickness);
        }
        else {
            material.SetFloat("_Thickness", 0);
        }
    }

    private void OnDisable() {
        material.SetFloat("_Thickness", 0);
    }
}
