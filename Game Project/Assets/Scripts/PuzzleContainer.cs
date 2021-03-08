using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleContainer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject completePuzzle;
    public GameObject puzzleOutline;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckComplete();
    }

    private void CheckComplete() {
        PuzzleController[] puzzles = GetComponentsInChildren<PuzzleController>();
        if(puzzles.Length == 0) {
            return;
        }
        foreach(PuzzleController puzzleController in puzzles) {
            if(!puzzleController.CheckPlaced()) {
                return;
            }
        }
        foreach(PuzzleController puzzleController in puzzles) {
            puzzleController.gameObject.SetActive(false);
        }
        puzzleOutline.SetActive(false);
        completePuzzle.SetActive(true);
        GetComponentInParent<PuzzleManager>().PuzzleCompleted();
    }
}
