using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startPage;
    public VideoPlayer videoPlayer;
    public GameObject slideBar;
    public RawImage dummy;
    public float loadingSpeed = 0.5f;
    private bool start = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start) {
            slideBar.GetComponentInChildren<Slider>().value += loadingSpeed * Time.deltaTime;
            if(slideBar.GetComponentInChildren<Slider>().value >= 1.0f) {
                SceneManager.LoadScene(1);
            }
        }
        if(Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene(1);
        }
    }

    public void StartIntro() {
        startPage.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.url = "https://projectblindspot.s3.amazonaws.com/Intro_with_subs.mp4";
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
        // videoPlayer.GetComponent<RawImage>().texture = rt;
        // videoPlayer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        // videoPlayer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        //ScalableBufferManager.ResizeBuffers(Screen.width, Screen.height);
        // videoPlayer.targetTexture.Release();
        // videoPlayer.targetTexture.width = Screen.width;
        // videoPlayer.targetTexture.height = Screen.height;
        // videoPlayer.targetTexture.Create();
        videoPlayer.Play();
        StartCoroutine(HoldForPlay());
    }

    IEnumerator HoldForPlay() {
        yield return new WaitForSeconds((float)videoPlayer.clip.length + 0.1f);
        start = true;
        slideBar.SetActive(true);
    }

    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
