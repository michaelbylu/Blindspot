using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleController : MonoBehaviour
{
    public Transform destination;
    public bool isBlinking = false;
    public float blinkSpeed = 1f;
    private Vector3 screenPoint;
    [SerializeField]
    private bool isPlaced = false;
    private bool isFadingIn = true;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Blink();
    }
    public void CheckPlaced() {
        if(Vector3.Distance(gameObject.transform.position, destination.position) <= 0.1f) {
            GetComponent<SpriteRenderer>().material.SetFloat("_Flag", 1f);
            GetComponent<BoxCollider2D>().enabled = false;
            isPlaced = true;
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 0f);
    }

    private void Blink() {
        if(!isBlinking) {
            return;
        }
        float thickness = GetComponent<SpriteRenderer>().material.GetFloat("_Thickness");
        if(isFadingIn) {
            thickness += blinkSpeed * Time.deltaTime;
            if(thickness >= 1.0f) {
                thickness = 1.0f;
                isFadingIn = false;
            }
        }
        else {
            thickness -= blinkSpeed * Time.deltaTime;
            if(thickness <= 0f) {
                thickness = 0f;
                isFadingIn = true;
            }
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", thickness);
    }

    public void StopBlinkng() {
        if(!isBlinking) {
            return;
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 1f);
        isBlinking = false;
    }

    public void StartBlinking() {
        if(isBlinking) {
            return;
        }
        isBlinking = true;
        isFadingIn = true;
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 0f);
    }


    private void OnMouseUp() {
        if(isPlaced) {
            return;
        }
        else {
            GetComponentInParent<ColorPuzzleManager>().PieceClicked(gameObject);
        }
    }

    public bool CheckStatus() {
        return isPlaced;
    }
}
