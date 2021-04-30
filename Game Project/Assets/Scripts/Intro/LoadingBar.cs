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
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Intro_with_vo_1.mp4");
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
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
