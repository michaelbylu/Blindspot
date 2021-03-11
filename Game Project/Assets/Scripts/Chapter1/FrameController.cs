using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMeshProUGUI;
    public TMPro.TextMeshProUGUI textMeshProNameTag;
    public float fadeSpeed;
    private RawImage frame;
    private bool isFadingOut;
    private bool isSwitching;
    private string targetText;
    private string nameTagText;
    private string targetTexture;
    // Start is called before the first frame update
    void Start()
    {
        frame = GetComponent<RawImage>();
    }

    private void OnEnable() {
        frame = GetComponent<RawImage>();
    }

    private void OnDisable() {
        frame = GetComponent<RawImage>();
        Color frameColor = frame.color;
        frameColor.a = 0f;
        frame.color = frameColor;
        Color textColor = textMeshProUGUI.color;
        textColor.a = 0f;
        textMeshProUGUI.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        FadeInOut();
    }

    private void FadeInOut() {
        if(!isSwitching) {
            return;
        }
        if(isFadingOut) {
            Color frameColor = frame.color;
            frameColor.a -= fadeSpeed * Time.deltaTime;
            if(frameColor.a <= 0) {
                isFadingOut = false;
                frameColor.a = 0f;
                frame.texture = Resources.Load<Texture2D>(targetTexture);
                textMeshProNameTag.text = nameTagText;
                textMeshProUGUI.text = targetText;
            }
            frame.color = frameColor;
            Color textColor = textMeshProUGUI.color;
            Color nameTagColor = textMeshProNameTag.color;
            
            textMeshProUGUI.color = new Color(textColor.r, textColor.g, textColor.b, frameColor.a);
            textMeshProNameTag.color = new Color(nameTagColor.r, nameTagColor.g, nameTagColor.b, frameColor.a);
        }
        else {
            Color frameColor = frame.color;
            frameColor.a += fadeSpeed * Time.deltaTime;
            if(frameColor.a >= 1) {
                isFadingOut = true;
                isSwitching = false;
                frameColor.a = 1f;
            }
            frame.color = frameColor;
            Color textColor = textMeshProUGUI.color;
            Color nameTagColor = textMeshProNameTag.color;
            textMeshProUGUI.color = new Color(textColor.r, textColor.g, textColor.b, frameColor.a);
            textMeshProNameTag.color = new Color(nameTagColor.r, nameTagColor.g, nameTagColor.b, frameColor.a);
        }
    }

    public void ChangeText(string text, string texture, bool isFading) {
        Debug.Log("text: " + text + " texture: " + texture + " " + isFadingOut.ToString());
        if(isFading) {
            isSwitching = true;
            isFadingOut = true;
            targetText = text;
            targetTexture = texture;
        }
        else {
            textMeshProUGUI.text = text;
            frame.texture = Resources.Load<Texture2D>(texture);
        }
    }

    public void ChangeNameTag(string name, bool isFading) {
        Debug.Log("name: " + name + " " + isFadingOut.ToString());
        if(isFading) {
            nameTagText = name;
        }
        else{
            textMeshProNameTag.text = name;
        }
    }
}
