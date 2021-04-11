using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPuzzleManager : MonoBehaviour
{
    public GameObject[] puzzles;
    public GameObject[] equations;
    private int placedPuzzle = 0;
    private bool isComplete = false;
    private AudioSource audioSource;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        CheckComplete();
    }

    private void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }

    private void CheckComplete() {
        if(placedPuzzle == equations.Length && !isComplete) {
            isComplete = true;
            StartCoroutine(ShowSolution());
            if(GameObject.FindGameObjectWithTag("Logger") != null) {
                Logger logger = GameObject.FindGameObjectWithTag("Logger").GetComponent<Logger>();
                logger.LogData(gameObject.name, (Time.time - startTime).ToString());
            }
        }
    }

    public void PuzzlePlaced() {
        StartCoroutine(EnableEquation());
    }

    IEnumerator ShowSolution() {
        // yield return new WaitForSeconds(2.0f);
        // for(int i=0; i<placedPuzzle; i++) {
        //     equations[i].GetComponent<FadeInOut>().StartFadingOut();
        // }
        // yield return new WaitForSeconds(2.0f);
        // equations[placedPuzzle].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        FadeInOut[] fadeInOuts = gameObject.GetComponentsInChildren<FadeInOut>();
        foreach(FadeInOut fadeInOut in fadeInOuts) {
            fadeInOut.StartFadingOut();
        }
        yield return new WaitForSeconds(2.0f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }

    IEnumerator EnableEquation() {
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        equations[placedPuzzle].SetActive(true);
        placedPuzzle++;
    }

}
