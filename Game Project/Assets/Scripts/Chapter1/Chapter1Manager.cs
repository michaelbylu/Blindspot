using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//This manager dude is gonna handle all the cheap n dirty code
//Wish him luck!
public class Chapter1Manager : MonoBehaviour
{
    public GameObject chair;
    public GameObject book;
    public GameObject puzzle1;
    public JsonReader jsonReader;
    private int currentStage = 0;
    private bool stageChanged = false;
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
            case 3: //When book is clicked, show the first puzzle
                puzzle1.SetActive(true);
                stageChanged = false;
                break;
            case 4: //When puzzle is completed, show next line
                jsonReader.ChangeLine("9");
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
}
