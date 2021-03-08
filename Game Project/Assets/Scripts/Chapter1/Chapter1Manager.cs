using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//This manager dude is gonna handle all the cheap n dirty code
//Wish him luck!
public class Chapter1Manager : MonoBehaviour
{
    public GameObject[] clocks;
    public GameObject book;
    public GameObject puzzle1_1;
    public GameObject puzzle1_2;
    public GameObject puzzle2_1;
    public GameObject puzzle2_2;
    public GameObject crowd;
    public JsonReader jsonReader;
    private int currentStage = 0;
    private string currentDifficulty = "A";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            currentStage = 9;
            CheckStage();
        }
    }

    private void CheckStage()
    {
        switch (currentStage)
        {
            case 1: //When player sit on the chair
                jsonReader.ChangeLine("0");
                break;
            case 2: //When first part dialogues end and the book is ready for click
                book.GetComponent<BookController>().EnableClick();
                break;
            case 3: //When book is clicked, show puzzle1-1
                puzzle1_1.SetActive(true);
                break;
            case 4: //When puzzle1-1 is completed, show next line
                jsonReader.ChangeLine("13");
                break;
            case 5: //When second part dialogues end and the book is ready for click again
                book.GetComponent<BookController>().EnableClick();
                break;
            case 6: //When book is clicked again, show puzzle1-2
                puzzle1_2.SetActive(true);
                break;
            case 7: //When puzzle1-2 is completed, show next line
                jsonReader.ChangeLine("21" + currentDifficulty);
                break;
            case 8: //When dialogues completed, turn on the clocks, crowd fadein
                clocks[0].SetActive(true);
                clocks[0].GetComponentInChildren<ClockController>().TurnOn(8);
                clocks[1].GetComponentInChildren<ClockController>().TurnOn(8);
                crowd.GetComponent<CrowdController>().EnableCrowd();
                break;
            case 9: //Start public class when crowd finish fading in
                jsonReader.ChangeLine("22");
                break;
            case 10: //Start puzzle2-1
                puzzle2_1.SetActive(true);
                break;
            case 11: // When puzzle2-1 completed, start the next dialogues
                jsonReader.ChangeLine("29");
                break;
            case 12: //Start puzzle2-2
                puzzle2_2.SetActive(true);
                break;                              
            default:
                break;
        }
    }

    public void ChangeStage()
    {
        currentStage++;
        CheckStage();
    }

    public void ChangeStage(string puzzleDifficulty) {
        currentStage++;
        CheckStage();
        currentDifficulty = puzzleDifficulty;
    }
}
