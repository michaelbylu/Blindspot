using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePuzzleManager : MonoBehaviour
{
    private bool isComplete = false;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckComplete() && !isComplete) {
            isComplete = true;
            StartCoroutine(FadeOut());
            if(GameObject.FindGameObjectWithTag("Logger") != null) {
                GameObject.FindGameObjectWithTag("Logger").GetComponent<Logger>().LogData("Puzzle2-2", (Time.time - startTime).ToString());
            }
        }
    }

    private bool CheckComplete() {
        foreach(DissolveController d in GetComponentsInChildren<DissolveController>()) {
            if(d.GetComponent<PolygonCollider2D>().enabled) {
                return false;
            }
        }
        return true;
    }

    IEnumerator FadeOut() {
        yield return new WaitForSeconds(6.5f);
        foreach(FadeInOut f in GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter3Manager>().ChangeStage();
    }
}
