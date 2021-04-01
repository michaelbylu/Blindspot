using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleManager : MonoBehaviour
{
    public GameObject[] colorPuzzles;
    private GameObject selectedOne;
    // Start is called before the first frame update
    void Start()
    {
        selectedOne = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PieceClicked(GameObject e) {
        if(selectedOne == null) {
            selectedOne = e;
            e.GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 1f);
        }
        else {
            e.GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 1f);
            StartCoroutine(SwitchPuzzles(e));
        }
    }

    IEnumerator SwitchPuzzles(GameObject e) {
        yield return new WaitForSeconds(1f);
        Vector3 pos = e.transform.position;
        e.transform.position = selectedOne.transform.position;
        selectedOne.transform.position = pos;
        e.GetComponent<ColorPuzzleController>().CheckPlaced();
        selectedOne.GetComponent<ColorPuzzleController>().CheckPlaced();
        selectedOne = null;
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
}
