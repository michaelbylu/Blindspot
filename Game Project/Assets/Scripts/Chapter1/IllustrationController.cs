using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationController : MonoBehaviour
{
    public float fadeSpeed;
    private RawImage rawImage;
    private string targetTexture;
    private bool isFadingOut;
    private bool isSwitching;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        FadeInOut();
    }

    private void OnEnable() {
        rawImage = GetComponent<RawImage>();
    }
    private void OnDisable() {
        Color color = rawImage.color;
        color.a = 0f;
        rawImage.color = color;
    }

    public void ChangeTexture(string texture) {
        if(rawImage.texture != Resources.Load<Texture2D>(texture)) {
            targetTexture = texture;
            isSwitching = true;
            isFadingOut = true;
            if(rawImage.texture == null) {
                Color color = rawImage.color;
                color.a = 0f;
                rawImage.color = color;
            }
        }
    }

    private void FadeInOut() {
        if(!isSwitching) {
            return;
        }
        if(isFadingOut) {
            Color color = rawImage.color;
            color.a -= fadeSpeed * Time.deltaTime;
            if(color.a <=0) {
                isFadingOut = false;
                color.a = 0f;
                rawImage.texture = Resources.Load<Texture2D>(targetTexture);
            }
            else {
                rawImage.color = color;
            }
        }
        else {
            Color color = rawImage.color;
            color.a += fadeSpeed * Time.deltaTime;
            if(color.a >=1) {
                isFadingOut = true;
                isSwitching = false;
                color.a = 1f;
            }
            else {
                rawImage.color = color;
            }
        }
    }
}
