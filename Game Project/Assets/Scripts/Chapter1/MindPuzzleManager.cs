using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPuzzleManager : MonoBehaviour
{
    public GameObject[] puzzles;
    public GameObject[] equations;
    private int placedPuzzle = 0;
    private bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckComplete();
    }

    private void CheckComplete() {
        if(placedPuzzle == equations.Length && !isComplete) {
            isComplete = true;
            StartCoroutine(ShowSolution());
        }
    }

    public void PuzzlePlaced() {
        equations[placedPuzzle].SetActive(true);
        placedPuzzle++;
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

}
