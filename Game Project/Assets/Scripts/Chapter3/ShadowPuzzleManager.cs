using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzleManager : MonoBehaviour
{
    public Vector3[] shadowWaypoints;
    public ShadowFigureController shadow;
    public GameObject[] crowd;
    private int stage = 0;
    // Start is called before the first frame update
    void Start()
    {
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
            StartCoroutine(FadeOut());
        }
        
    }

    IEnumerator StartPuzzle() {
        yield return new WaitForSeconds(2f);
        shadow.SetDestination(shadowWaypoints[0]);
    }

    IEnumerator EnableNextCrowd() {
        shadow.GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(1f);
        crowd[stage - 1].GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(2f);
        crowd[stage].SetActive(true);
        yield return new WaitForSeconds(2f);
        shadow.SetDestination(shadowWaypoints[stage]);
        shadow.GetComponent<FadeInOut>().StartFadingIn();
    }

    IEnumerator FadeOut() {
        yield return new WaitForSeconds(2f);
        foreach(FadeInOut f in GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter3Manager>().ChangeStage();
    }
}
