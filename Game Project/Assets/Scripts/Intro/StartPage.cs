using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    public GameObject startButton;
    public GameObject idleMeimei;
    public VideoPlayer videoPlayer;
    public GameObject loadingBar;
    public GameObject startPage;
    private bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            SceneManager.LoadScene(2);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(3);
        }
        if(!isComplete && CheckVideoComplete()) {
            isComplete = true;
            startPage.SetActive(true);
            idleMeimei.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
        }
    }

    private bool CheckVideoComplete() {
        if(!videoPlayer.gameObject.activeSelf) {
            return false;
        }
        if(!videoPlayer.isPlaying && videoPlayer.frame > (long)videoPlayer.frameCount - 5) {
            return true;
        }
        return false;
    }

    IEnumerator EnableTransition() {
        yield return new WaitForSeconds(0.1f);
    }

    public void StartTransition() {
        loadingBar.SetActive(true);
        startPage.SetActive(false);
        idleMeimei.SetActive(false);
    }

    public void StartPrologue() {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Prologue_final.mp4");
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
        videoPlayer.Play();
        startButton.SetActive(false);
    }
}
