using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class VideoController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPlayed = false;
    private VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.isPlaying && !isPlayed) {
            isPlayed = true;
        }
        if(isPlayed && !videoPlayer.isPlaying) {
            if(GameObject.FindGameObjectWithTag("GameController").name.StartsWith("Chapter1")) {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
                PlayerPrefs.SetInt("1", 1);
            }
            else if(GameObject.FindGameObjectWithTag("GameController").name.StartsWith("Chapter2")){
                GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
                PlayerPrefs.SetInt("2", 1);
            } 
            else if(GameObject.FindGameObjectWithTag("GameController").name.StartsWith("Chapter3")){
                PlayerPrefs.SetInt("3", 1);
            }
            gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }

    private void OnEnable() {
        videoPlayer = GetComponent<VideoPlayer>();
    }
}
