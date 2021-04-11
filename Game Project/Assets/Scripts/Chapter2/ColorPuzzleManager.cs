using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleManager : MonoBehaviour
{
    public GameObject[] colorPuzzles;
    public GameObject[] BlinkingPuzzles;
    private GameObject selectedOne;
    private bool isCompleted;
    private bool clickable = true;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        selectedOne = null;
        foreach(GameObject e in colorPuzzles) {
            e.GetComponent<PolygonCollider2D>().enabled = false;
        }
        foreach(GameObject e in BlinkingPuzzles) {
            e.GetComponent<PolygonCollider2D>().enabled = true;
            e.GetComponent<ColorPuzzleController>().StartBlinking();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCompleted && CheckComplete()) {
            isCompleted = true;
            StartCoroutine(FadeOut());
            if(GameObject.FindGameObjectWithTag("Logger") != null) {
                Logger logger = GameObject.FindGameObjectWithTag("Logger").GetComponent<Logger>();
                logger.LogData(gameObject.name, (Time.time - startTime).ToString());
            }
        }
    }

    public void PieceClicked(GameObject e) {
        if(!clickable) {
            return;
        }
        if(selectedOne == null) {
            selectedOne = e;
            e.GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 1f);
            e.GetComponent<ColorPuzzleController>().StopBlinkng();
        }
        else {
            if(e.GetComponent<ColorPuzzleController>().isBlinking) {
                foreach(GameObject t in colorPuzzles) {
                    t.GetComponent<PolygonCollider2D>().enabled = true;
                }
            }
            e.GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 1f);
            e.GetComponent<ColorPuzzleController>().StopBlinkng();
            StartCoroutine(SwitchPuzzles(e));
        }
    }

    IEnumerator SwitchPuzzles(GameObject e) {
        clickable = false;
        yield return new WaitForSeconds(1f);
        Vector3 pos = e.transform.position;
        e.transform.position = selectedOne.transform.position;
        selectedOne.transform.position = pos;
        e.GetComponent<ColorPuzzleController>().CheckPlaced();
        selectedOne.GetComponent<ColorPuzzleController>().CheckPlaced();
        selectedOne = null;
        clickable = true;
    }

    public bool CheckComplete() {
        foreach(GameObject puzzle in colorPuzzles) {
            if(puzzle.GetComponent<ColorPuzzleController>() != null) {
                if(puzzle.GetComponent<ColorPuzzleController>().CheckStatus() == false){
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator FadeOut() {
        yield return new WaitForSeconds(4f);
        foreach(FadeInOut f in transform.GetComponentsInChildren<FadeInOut>()){
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
    }
}
