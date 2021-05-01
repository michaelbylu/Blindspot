using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzleManager : MonoBehaviour
{
    public Vector3[] shadowWaypoints;
    public ShadowFigureController shadow;
    public GameObject[] crowd;
    private int stage = 0;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        StartCoroutine(StartPuzzle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReleasePlayer() {
        GetComponentInChildren<ColoredFigureController>().SetMovable(true); 
    }

    public void FreezePlayer() {
        GetComponentInChildren<ColoredFigureController>().SetMovable(false);
        stage++;
        if(stage < crowd.Length) {
            StartCoroutine(EnableNextCrowd());
        }
        else {
            if(GameObject.FindGameObjectWithTag("Logger") != null) {
                GameObject.FindGameObjectWithTag("Logger").GetComponent<Logger>().LogData("Puzzle1", (Time.time - startTime).ToString());
            }
            StartCoroutine(FadeOut());
        }
        
    }

    IEnumerator StartPuzzle() {
        yield return new WaitForSeconds(2f);
        shadow.SetDestination(shadowWaypoints[0]);
    }

    IEnumerator EnableNextCrowd() {
        if(shadow.marker.activeSelf) {
            shadow.marker.SetActive(false);
        }
        shadow.GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(3f);
        crowd[stage - 1].GetComponent<Bubble>().FadeOut();
        yield return new WaitForSeconds(1f);
        crowd[stage - 1].GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(2f);
        crowd[stage].SetActive(true);
        crowd[stage - 1].SetActive(false);
        yield return new WaitForSeconds(2f);
        shadow.SetDestination(shadowWaypoints[stage]);
        shadow.GetComponent<FadeInOut>().StartFadingIn();
    }

    IEnumerator FadeOut() {
        shadow.GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(2f);
        crowd[stage - 1].GetComponent<Bubble>().FadeOut();
        yield return new WaitForSeconds(2f);
        foreach(FadeInOut f in GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter3Manager>().ChangeStage();
    }
}
