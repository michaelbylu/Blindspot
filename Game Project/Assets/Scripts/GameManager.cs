using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TimelineAsset additionalDialogues;

    public GameObject puzzle1;
    public GameObject puzzle2;

    public GameObject TextCanvas;
    public GameObject ImageCanvas;

    private PlayableDirector playableDirector;
    void Start()
    {
        playableDirector = gameObject.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAdditionalDialogue() {
        playableDirector.playableAsset = additionalDialogues;
        playableDirector.Play();
        StartCoroutine(Sleep2(6));
    }

    public void ShowDialogue() {
        StartCoroutine(Sleep1(2));        
    }

    public void StartPuzzle2() {
        TextCanvas.SetActive(false);
        ImageCanvas.SetActive(false);
        puzzle2.SetActive(true);
    }

    IEnumerator Sleep1(float time) {
        yield return new WaitForSeconds(time);
        puzzle1.SetActive(false);
        playableDirector.Play();
    }

    IEnumerator Sleep2(float time) {
        yield return new WaitForSeconds(time);
        StartPuzzle2();
    }
}
