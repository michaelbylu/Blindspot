using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//This manager dude is gonna handle all the cheap n dirty code
//Wish him luck!
public class Chapter1Manager : MonoBehaviour
{
    public GameObject clock;
    public GameObject book;
    public GameObject puzzle1_1;
    public GameObject puzzle1_2;
    public JsonReader jsonReader;
    private int currentStage = 0;
    private bool stageChanged = false;
    private int currentDifficulty = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stageChanged)
        {
            CheckStage();
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            currentStage = 8;
            CheckStage();
        }
    }

    private void CheckStage()
    {
        switch (currentStage)
        {
            case 1: //When player sit on the chair
                jsonReader.ChangeLine("0");
                stageChanged = false;
                break;
            case 2: //When first part dialogues end and the book is ready for click
                book.GetComponent<BookController>().EnableClick();
                stageChanged = false;
                break;
            case 3: //When book is clicked, show puzzle1-1
                puzzle1_1.SetActive(true);
                stageChanged = false;
                break;
            case 4: //When puzzle1-1 is completed, show next line
                jsonReader.ChangeLine("9");
                stageChanged = false;
                break;
            case 5: //When second part dialogues end and the book is ready for click again
                book.GetComponent<BookController>().EnableClick();
                stageChanged = false;
                break;
            case 6: //When book is clicked again, show puzzle1-2
                puzzle1_2.SetActive(true);
                stageChanged = false;
                break;
            case 7: //When puzzle1-2 is completed, show next line
                jsonReader.ChangeLine("9");
                stageChanged = false;
                break;
            case 8: //When dialogues completed, turn on the clock
                clock.SetActive(true);
                clock.GetComponentInChildren<ClockController>().TurnOn(5);
                stageChanged = false;
                break;
            default:
                stageChanged = false;
                break;
        }
    }

    public void ChangeStage()
    {
        currentStage++;
        stageChanged = true;
    }

    public void ChangeStage(int puzzleDifficulty) {
        currentStage++;
        stageChanged = true;
        currentDifficulty = puzzleDifficulty;
    }
}
