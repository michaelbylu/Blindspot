using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleController : MonoBehaviour
{
    public Transform destination;
    private Vector3 screenPoint;
    private bool isPlaced = false;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckPlaced() {
        if(Vector3.Distance(gameObject.transform.position, destination.position) <= 0.1f) {
            GetComponent<SpriteRenderer>().material.SetFloat("_Flag", 1f);
            GetComponent<PolygonCollider2D>().enabled = false;
            isPlaced = true;
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 0f);
    }


    private void OnMouseUp() {
        if(isPlaced) {
            return;
        }
        else {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ColorPuzzleManager>().PieceClicked(gameObject);
        }
    }

    public bool CheckStatus() {
        return isPlaced;
    }
}
