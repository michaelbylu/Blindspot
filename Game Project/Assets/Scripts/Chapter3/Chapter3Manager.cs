using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter3Manager : MonoBehaviour
{
    public Logger logger;
    public GameObject flashback;
    public PlayableAsset[] timelines;
    public PlayerController player;
    public GameObject puzzle1;
    public Transform[] waypoints;
    public GameObject[] puzzle2;
    public GameObject blocks;
    public JsonReader jsonReader;
    private string currentDifficulty;
    private int currentStage = 0;
    public float fadeSpeed = 0.5f;
    private AudioSource audioSource;
    private float targetVolume;

    private bool isFadingOut = false;
    private bool isFadingIn = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeVolume();
        if(Input.GetKeyDown(KeyCode.R) && currentStage >= 12) {
            SceneManager.LoadScene(3);
        }
        if(Input.GetKeyDown(KeyCode.P) && currentStage >= 12) {
            SceneManager.LoadScene(0);
        }
    }

    private void ChangeVolume() {
        if(isFadingOut) {
            audioSource.volume -= fadeSpeed * Time.deltaTime;
            if(audioSource.volume <= targetVolume) {
                audioSource.volume = targetVolume;
                isFadingOut = false;
            }
        }
        else if(isFadingIn) {
            audioSource.volume += fadeSpeed * Time.deltaTime;
            if(audioSource.volume >= targetVolume) {
                audioSource.volume = targetVolume;
                isFadingIn = false;
            }
        }
    }

    private void CheckStage() {
        switch(currentStage) {
            case 1: //Start dialogue
                jsonReader.ChangeLine("0");
                blocks.SetActive(false);
                break;
            case 2: //After Meimei had conversation with Kevin and Emma, enable puzzle1, let Kevin/Emma join the karaoke
                puzzle1.SetActive(true);
                GetComponent<PlayableDirector>().Play();
                break;
            case 3: //When puzzle1 is completed, let player walk around the scene
                puzzle1.SetActive(false);
                player.chair.position = waypoints[0].position;
                player.Release();
                player.SwitchWalkAnimation(1);
                break;
            case 4: //After Meimei is sit, start cinematics
                GetComponent<PlayableDirector>().playableAsset = timelines[0];
                GetComponent<PlayableDirector>().Play();
                break;
            case 5: //After Frank walks in, continue the dialogue
                jsonReader.ChangeLine("22");
                break;
            case 6: //Frank leaves and Grace joins the conversation
                GetComponent<PlayableDirector>().playableAsset = timelines[1];
                GetComponent<PlayableDirector>().Play();
                break;
            case 7: //Start conversation with Grace
                jsonReader.ChangeLine("28");
                break;
            case 8: //Enable puzzle2 based on options
                if(currentDifficulty == "A") {
                    puzzle2[0].SetActive(true);
                }
                else if(currentDifficulty == "B") {
                    puzzle2[1].SetActive(true);
                }
                break;
            case 9: //Continue conversation after puzzle is completed
                if(currentDifficulty == "A") {
                    puzzle2[0].SetActive(false);
                    jsonReader.ChangeLine("43c");
                }
                else if(currentDifficulty == "B") {
                    puzzle2[1].SetActive(false);
                    jsonReader.ChangeLine("42c");
                }
                break;
            case 10: //Let player walk to the karaoke，Grace starts walking as well
                player.chair.position = waypoints[1].position;
                player.Release();
                player.SwitchWalkAnimation(2);
                GetComponent<PlayableDirector>().playableAsset = timelines[2];
                GetComponent<PlayableDirector>().Play();
                break;
            case 11: //Play the ending timeline
                jsonReader.ChangeLine("44");
                break;
            case 12:
                foreach(ParticleSystem p in player.GetComponentsInChildren<ParticleSystem>(true)){
                    p.gameObject.SetActive(true);
                }
                StartCoroutine(EnableOutro());
                break;
            case 13:
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
    }

    public void ChangeStage()
    {
        currentStage++;
        CheckStage();
    }

    public void ChangeStage(string puzzleDifficulty) {
        currentStage++;
        currentDifficulty = puzzleDifficulty;
        CheckStage();
    }

    public void AudioFadeOut(float volume) {
        isFadingOut = true;
        targetVolume = volume;
    }

    public void AudioFadeIn(float volume) {
        isFadingIn = true;
        targetVolume = volume;
    }

    IEnumerator EnableOutro() {
        yield return new WaitForSeconds(6f);
        if(logger != null) {
            logger.SendLogs();
        }
        AudioFadeOut(0f);
        foreach(SpriteRenderer f in GameObject.FindObjectsOfType<SpriteRenderer>()) {
            if(f.GetComponent<FadeInOut>() == null) {
                FadeInOut fadeInOut = f.gameObject.AddComponent<FadeInOut>();
                fadeInOut.speed = 1f;
                fadeInOut.StartFadingOut();
            }
            else {
                f.GetComponent<FadeInOut>().speed = 1f;
                f.GetComponent<FadeInOut>().StartFadingOut();
            }
        }
        foreach(ParticleSystem p in GameObject.FindObjectsOfType<ParticleSystem>()) {
            p.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        flashback.SetActive(true);
        VideoPlayer videoPlayer = flashback.GetComponent<VideoPlayer>();
        if(currentDifficulty == "A" || currentDifficulty == "B") {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Chapter3FBandEnding_finalfinal.mp4");                   
        }
        else {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Chapter3FBandEnding_finalfinal.mp4");
        }
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
        yield return new WaitForSeconds(2f);
        flashback.GetComponent<VideoPlayer>().Play();
    }
}
