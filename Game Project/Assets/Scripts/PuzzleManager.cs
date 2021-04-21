using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Time interval between each bubble puzzle is generated
    public float bubbleInterval = 3f;
    public GameObject[] puzzles;
    public GameObject[] chatBubbles;
    public GameObject[] otherBubbles;
    public int num = 0;
    private int completedPuzzles = 0;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        StartCoroutine(EnableNext());
    }

    public void PuzzleCompleted() {
        completedPuzzles++;
        StartCoroutine(EnableNext());
    }

    IEnumerator EnableNext() {
        int count = Random.Range(num - 1, num + 1);
        //Show the completed chat bubble
        if(completedPuzzles > 0) {
            yield return new WaitForSeconds(bubbleInterval/2);
            chatBubbles[completedPuzzles - 1].SetActive(true);
            chatBubbles[completedPuzzles - 1].GetComponent<FadeInOut>().StartFadingIn();
            yield return new WaitForSeconds(bubbleInterval);
            chatBubbles[completedPuzzles - 1].GetComponent<FadeInOut>().StartFadingOut();
        }
        //Let M/F talk for a few rounds
        for(int i=0; i<count; i++) {
            yield return new WaitForSeconds(1f);
            int rng = Random.Range(0, otherBubbles.Length);
            otherBubbles[rng].SetActive(true);
            yield return new WaitForSeconds(1.5f);
            otherBubbles[rng].GetComponent<FadeInOut>().StartFadingOut();
            yield return new WaitForSeconds(1f);
            otherBubbles[rng].SetActive(false);

        }
        if(completedPuzzles < puzzles.Length) {
            puzzles[completedPuzzles].SetActive(true);
            if(completedPuzzles > 0) {
                puzzles[completedPuzzles - 1].SetActive(false);
            }
        }
        else {
            puzzles[completedPuzzles - 1].SetActive(false);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut() {
        if(GameObject.FindGameObjectWithTag("Logger") != null) {
            Logger logger = GameObject.FindGameObjectWithTag("Logger").GetComponent<Logger>();
            logger.LogData(gameObject.name, (Time.time - startTime).ToString());
        }
        FadeInOut[] fadeInOuts = gameObject.GetComponentsInChildren<FadeInOut>();
        foreach(FadeInOut fadeInOut in fadeInOuts) {
            fadeInOut.StartFadingOut();
        }
        yield return new WaitForSeconds(3f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }
}
