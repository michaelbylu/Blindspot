using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;

    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool isBlinking = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadingIn) {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a += Time.deltaTime * speed;
            if(color.a >= 1f) {
                color.a = 1f;
                isFadingIn = false;
                if(isBlinking) {
                    isFadingOut = true;
                }
            }
            GetComponent<SpriteRenderer>().color = color;
        }
        if(isFadingOut) {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a -= Time.deltaTime * speed;
            if(color.a <= 0f) {
                color.a = 0f;
                isFadingOut = false;
                if(isBlinking) {
                    isFadingIn = true;
                }
            }
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void OnEnable() {
        isFadingIn = true;
    }

    public void StartFadeIn() {
        isFadingIn = true;
    }

    public void StartFadingOut() {
        isFadingOut = true;
    }

    public void StartBlinking() {
        isBlinking = true;
        isFadingOut = true;
    }

    public void StopBlinking() {
        isBlinking = false;
        isFadingIn = false;
        isFadingOut = false;
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }
}
