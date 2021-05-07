using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter2Manager : MonoBehaviour
{
    public GameObject frank;
    public GameObject jasmine;
    public GameObject puzzle1_1;
    public GameObject puzzle1_2;
    public GameObject markers;
    public GameObject food;
    public GameObject puzzle2_1;
    public GameObject puzzle2_2;
    public GameObject dumplingInteraction;
    public GameObject flashback;
    public GameObject endPage;
    public JsonReader jsonReader;
    public GameObject block;
    public Logger logger;
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
        if(Input.GetKeyDown(KeyCode.F10)) {
            ChangeStage();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentStage >= 14) {
            SceneManager.LoadScene(2);
        }
        if(Input.GetKeyDown(KeyCode.P) && currentStage >= 14) {
            SceneManager.LoadScene(0);
        }
        ChangeVolume();
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

    private void CheckStage()
    {
        switch (currentStage)
        {
            case 1: //When player sit on the chair
                jsonReader.ChangeLine("0");
                block.SetActive(false);
                break;
            case 2: //Active puzzle1-1
                AudioFadeOut(0);
                puzzle1_1.SetActive(true);
                break;
            case 3: //Continue the conversation after puzzle1-1
                AudioFadeIn(0.4f);
                jsonReader.ChangeLine("4");
                puzzle1_1.SetActive(false);
                break;
            case 4: //Let Frank walk in
                frank.SetActive(true);
                break;
            case 5: //Resume the conversation after Frank joins.
                jsonReader.ChangeLine("10");
                break;
            case 6: //Active puzzle1-2
                AudioFadeOut(0);
                puzzle1_2.SetActive(true);
                break;
            case 7: //Continue the conversation after puzzle1-2
                AudioFadeIn(0.4f);
                puzzle1_2.SetActive(false);
                jsonReader.ChangeLine("22");
                break;
            case 8: //People start getting their food
                StartCoroutine(StartGrabFood());
                markers.SetActive(true);
                break;
            case 9:
                if(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().GetBool("isSit")) {
                    food.SetActive(true);
                }
                break;
            case 10:
                if(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().GetBool("isSit")) {
                    food.SetActive(true);
                }
                break;
            case 11: //Meimei got her food and return to the table, resume dialogue
                jsonReader.ChangeLine("31");
                markers.SetActive(false);
                food.SetActive(true);
                break;
            case 12: //Enable puzzle2-1 or 2-2 based on the choice
                if(currentDifficulty == "A" || currentDifficulty == "B") {
                    puzzle2_1.SetActive(true);
                }
                else {
                    puzzle2_2.SetActive(true);
                }
                break;
            case 13: //Puzzle cleared, resume to dialogue
                if(currentDifficulty == "A") {
                    jsonReader.ChangeLine("38a");
                    puzzle2_1.SetActive(false);
                }
                else if(currentDifficulty == "B") {
                    jsonReader.ChangeLine("39a");
                    puzzle2_1.SetActive(false);
                }
                else {
                    jsonReader.ChangeLine("40a");
                    puzzle2_2.SetActive(false);
                    currentStage = currentStage + 2;
                }
                break;
            case 14: //Active dumplings interaction
                dumplingInteraction.SetActive(true);
                break;
            case 15:
                dumplingInteraction.SetActive(false);
                jsonReader.ChangeLine("afterSharingDumpling");
                break;
            case 16: 
                StartCoroutine(EnableOutro());
                break;
            case 17:
                endPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    IEnumerator StartGrabFood() {
        markers.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Release();
        jasmine.GetComponent<NPCController>().StartGrabFood(-8);
        yield return new WaitForSeconds(3f);
        frank.GetComponent<NPCController>().StartGrabFood(-7);
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
        if(logger != null) {
            logger.SendLogs();
        }
        AudioFadeOut(0f);
        foreach(FadeInOut f in GameObject.FindObjectsOfType<FadeInOut>()) {
            f.speed = 1f;
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        flashback.SetActive(true);
        VideoPlayer videoPlayer = flashback.GetComponent<VideoPlayer>();
        if(currentDifficulty == "A" || currentDifficulty == "B") {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Chapter2PositiveFB_final.mp4");                   
        }
        else {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Chapter2NegativeFB_final.mp4");
        }
        RenderTexture rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.Create();
        videoPlayer.targetTexture = rt;
        videoPlayer.GetComponent<RawImage>().texture = rt;
        yield return new WaitForSeconds(2f);
        flashback.GetComponent<VideoPlayer>().Play();
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }
}
