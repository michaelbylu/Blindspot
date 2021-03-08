using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    // Fade in/out speed
    public float speed = 2f;

    // For object that has isBlinking, the interval between fade in/out
    public float blinkInterval = 0.01f;
    public bool blinkAtStart = false;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool isBlinking = false;
    void Start()
    {
        if(blinkAtStart) {
            StartBlinking();
        }
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
                    StartCoroutine(WaitForFadeOut());
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
                    StartCoroutine(WaitForFadeIn());
                }
            }
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void OnEnable() {
        isFadingIn = true;
        if(blinkAtStart) {
            isFadingIn = false;
            StartBlinking();
        }
    }

    public void StartFadingIn() {
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
        StopAllCoroutines();
    }

    IEnumerator WaitForFadeOut() {
        yield return new WaitForSeconds(Random.Range(0.8f * blinkInterval, 1.2f * blinkInterval));
        isFadingOut = true;
    }

    IEnumerator WaitForFadeIn() {
        yield return new WaitForSeconds(Random.Range(0.8f * blinkInterval, 1.2f * blinkInterval));
        isFadingIn = true;
    }
}
