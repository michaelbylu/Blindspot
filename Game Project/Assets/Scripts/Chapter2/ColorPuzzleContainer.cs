using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzleContainer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckComplete() && !spriteRenderer.enabled) {
            spriteRenderer.enabled = true;
            StartCoroutine(EnableCover());
            foreach(ColorPuzzleController puzzle in GetComponentsInChildren<ColorPuzzleController>()) {
                puzzle.gameObject.SetActive(false);
            }
        }
    }

    private bool CheckComplete() {
        foreach(ColorPuzzleController puzzle in GetComponentsInChildren<ColorPuzzleController>()) {
            if(puzzle.CheckStatus() == false){
                return false;
            }
        }
        return true;
    }

    IEnumerator EnableCover() {
        yield return new WaitForSeconds(2f);
        transform.Find("Cover").gameObject.SetActive(true);
    }
}
