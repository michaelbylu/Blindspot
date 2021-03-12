using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

//This manager dude is gonna handle all the cheap n dirty code
//Wish him luck!
public class Chapter1Manager : MonoBehaviour
{
    public GameObject[] clocks;
    public GameObject book;
    public GameObject puzzle1_1;
    public GameObject puzzle1_2;
    public GameObject puzzle2_1;
    public GameObject[] puzzle2_2;
    public GameObject flashback;
    public GameObject endPage;
    public GameObject crowd;
    public JsonReader jsonReader;
    private int currentStage = 0;
    private string currentDifficulty = "A";
    public float fadeSpeed = 0.5f;
    private AudioSource audioSource;
    private float targetVolume;

    private bool isFadingOut = false;
    private bool isFadingIn = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            currentStage = 14;
            CheckStage();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentStage >= 15) {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.P) && currentStage >= 15) {
            SceneManager.LoadScene(0);
        }
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

    private void CheckStage()
    {
        switch (currentStage)
        {
            case 1: //When player sit on the chair
                jsonReader.ChangeLine("0");
                break;
            case 2: //When first part dialogues end and the book is ready for click
                book.GetComponent<BookController>().EnableClick();
                break;
            case 3: //When book is clicked, show puzzle1-1
                puzzle1_1.SetActive(true);
                break;
            case 4: //When puzzle1-1 is completed, show next line
                jsonReader.ChangeLine("13");
                break;
            case 5: //When second part dialogues end and the book is ready for click again
                book.GetComponent<BookController>().EnableClick();
                puzzle1_1.SetActive(false);
                break;
            case 6: //When book is clicked again, show puzzle1-2
                puzzle1_2.SetActive(true);
                break;
            case 7: //When puzzle1-2 is completed, show next line
                StartCoroutine(CloseBook());
                break;
            case 8: //When dialogues completed, turn on the clocks, crowd fadein
                StartCoroutine(EnableTransition());
                break;
            case 9: //Start public class when crowd finish fading in
                AudioFadeIn(0.4f);
                jsonReader.ChangeLine("22");
                break;
            case 10: //Start puzzle2-1
                puzzle2_1.SetActive(true);
                clocks[1].GetComponentInChildren<ClockController>().TurnOn(4);
                break;
            case 11: // When puzzle2-1 completed, start the next dialogues
                jsonReader.ChangeLine("29");
                break;
            case 12: //Start puzzle2-2
                puzzle2_1.SetActive(false);
                if(currentDifficulty == "C" || currentDifficulty == "B") {
                    puzzle2_2[0].SetActive(true);
                }
                else {
                    puzzle2_2[1].SetActive(true);
                }
                break;
            case 13: //Line before flashback
                jsonReader.ChangeLine("33" + currentDifficulty);
                break;
            case 14: //Play flashback based on previous choices
                StartCoroutine(EnableOutro());
                break;
            case 15: //End page (for now)
                endPage.SetActive(true);
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
        AudioFadeOut(0f);
        flashback.SetActive(true);
        VideoPlayer videoPlayer = flashback.GetComponent<VideoPlayer>();
        if(jsonReader.CheckLog("19") && jsonReader.CheckLog("puzzle2-2A")) {
            videoPlayer.url = 
                "https://projectblindspot.s3.amazonaws.com/Chapter1NegativeFB_with_subs.mp4";                   
        }
        else {
            videoPlayer.url = 
                "https://projectblindspot.s3.amazonaws.com/Chapter1PositiveFB_with_subs.mp4";
        }
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
        yield return new WaitForSeconds(2f);
        flashback.GetComponent<VideoPlayer>().Play();
    }

    IEnumerator EnableTransition() {
        AudioFadeOut(0.2f);
        yield return new WaitForSeconds(2f);
        clocks[0].SetActive(true);
        clocks[0].GetComponentInChildren<ClockController>().TurnOn(8);
        clocks[1].GetComponentInChildren<ClockController>().TurnOn(8);
        crowd.GetComponent<CrowdController>().EnableCrowd();
    }

    IEnumerator CloseBook() {
        yield return new WaitForSeconds(1f);
        book.GetComponent<BookController>().CloseBook();
        yield return new WaitForSeconds(2f);
        puzzle1_2.SetActive(false);
        jsonReader.ChangeLine("21" + currentDifficulty);
    }

}
