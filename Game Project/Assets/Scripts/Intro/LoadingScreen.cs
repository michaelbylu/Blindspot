using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public int nextSceneIndex;
    public bool continueous = false;
    public TMPro.TextMeshProUGUI textMeshProUGUI;
    public string[] texts;
    public float textFlashInterval;
    public GameObject fillingBar;
    public float fillingTime;
    public float fillingTarget;
    public float[] fillingPercents;
    public float movingTarget;
    public GameObject[] Meimei;
    private float lastFlash;
    private int textIndex;
    private bool isFilling = false;
    private float fillingProgress;
    private int currentTexture;
    private bool previousFading = false;
    // Start is called before the first frame update
    void Start()
    {
        lastFlash = Time.time;
        textIndex = 0;
        textMeshProUGUI.text = texts[0];
        StartFilling();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            StartFilling();
        }
        TextFlash();
        Filling();
    }

    private void TextFlash() {
        if(Time.time - lastFlash >= textFlashInterval) {
            textIndex++;
            textIndex = textIndex % texts.Length;
            textMeshProUGUI.text = texts[textIndex];
            lastFlash = Time.time;
        }
    }

    private void Filling() {
        if(!isFilling) {
            return;
        }
        fillingBar.transform.localScale = new Vector3(fillingBar.transform.localScale.x + Time.deltaTime * fillingTarget / fillingTime,
            1f, 1f);
        fillingProgress = fillingBar.transform.localScale.x / fillingTarget;
        if(fillingProgress >= 1) {
            isFilling = false;
            if(nextSceneIndex >= 0 && nextSceneIndex <=3) {
                SceneManager.LoadScene(nextSceneIndex);
            }
            return;
        }
        if(continueous) {
            Meimei[0].transform.position += new Vector3(Time.deltaTime * movingTarget / fillingTime, 0f, 0f);
            if(fillingProgress > fillingPercents[currentTexture] && currentTexture < fillingPercents.Length - 1) {
                currentTexture++;
                Meimei[0].GetComponent<SpriteRenderer>().sprite = Meimei[currentTexture].GetComponent<SpriteRenderer>().sprite;
            }
        }
        else {
            if(fillingProgress > fillingPercents[currentTexture] && !previousFading) {
                previousFading = true;
                Meimei[currentTexture].GetComponent<FadeInOut>().StartFadingOut();
            }
            if(previousFading && Meimei[currentTexture].GetComponent<SpriteRenderer>().color.a <= 0.01f) {
                Meimei[currentTexture].SetActive(false);
                currentTexture++;
                Meimei[currentTexture].SetActive(true);
                Meimei[currentTexture].GetComponent<FadeInOut>().StartFadingIn();
                previousFading = false;
            }
        }
        
    }

    public void StartFilling() {
        isFilling = true;
        Meimei[0].SetActive(true);
        Meimei[0].GetComponent<FadeInOut>().StartFadingIn();
        currentTexture = 0;
        textMeshProUGUI.gameObject.SetActive(true);
    }

    IEnumerator StartTransition() {
        yield return new WaitForEndOfFrame();
    }
}
