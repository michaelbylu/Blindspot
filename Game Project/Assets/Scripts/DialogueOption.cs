using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Option(int optionIndex) {
        if(optionIndex == 1) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().StartPuzzle2();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if(optionIndex == 2 || optionIndex == 3) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ShowAdditionalDialogue();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
