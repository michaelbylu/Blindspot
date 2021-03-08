using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Time interval between each bubble puzzle is generated
    public float bubbleInterval = 3f;
    public GameObject[] puzzles;
    public GameObject chatBubble;
    private int completedPuzzles = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        puzzles[completedPuzzles].SetActive(true);
    }

    public void PuzzleCompleted() {
        completedPuzzles++;
        StartCoroutine(EnableNext());
    }

    IEnumerator EnableNext() {
        yield return new WaitForSeconds(1f);
        chatBubble.SetActive(true);
        chatBubble.GetComponent<FadeInOut>().StartFadingIn();
        yield return new WaitForSeconds(bubbleInterval);
        chatBubble.GetComponent<FadeInOut>().StartFadingOut();
        if(completedPuzzles < puzzles.Length) {
            puzzles[completedPuzzles].SetActive(true);
            puzzles[completedPuzzles - 1].SetActive(false);
        }
        else {
            puzzles[completedPuzzles - 1].SetActive(false);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut() {
        FadeInOut[] fadeInOuts = gameObject.GetComponentsInChildren<FadeInOut>();
        foreach(FadeInOut fadeInOut in fadeInOuts) {
            Debug.Log(fadeInOut.gameObject.name);
            fadeInOut.StopBlinking();
            fadeInOut.StartFadingOut();
        }
        yield return new WaitForSeconds(3f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }
}
