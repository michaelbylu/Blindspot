using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonReader : MonoBehaviour
{
    public int chapterIndex = 1;
    public TextAsset linesJson;
    public TextAsset optionsJson;
    public GameObject nextBtn;
    public GameObject lineObject;
    private string currentLine = "0";
    private string nextLine = "";
    private Lines testLines;
    private Options testOptions;
    private ArrayList lineLog;
    // Start is called before the first frame update
    void Start()
    {
        testLines = JsonUtility.FromJson<Lines>(linesJson.text);
        testOptions = JsonUtility.FromJson<Options>(optionsJson.text);
        lineLog = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            JumpToNext();
        }
    }

    //Switch dialogue content based on targetIndex
    //When targetIndex indicate options, turn on option buttons
    public void ChangeLine(string targetIndex) {
        if(targetIndex.StartsWith("puzzle")) {
            lineObject.SetActive(false);
            if(targetIndex.Length > "puzzlex-x".Length) {
                string level = targetIndex[9].ToString();
                if(chapterIndex == 1) {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage(level);
                }
                else if(chapterIndex == 2) {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage(level);
                }
                
            }
            else{
                if(chapterIndex == 1) {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
                }
                else if(chapterIndex == 2) {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
                }
            }
            return;
        }
        if(targetIndex == "options") {
            EnableOptions(currentLine);
            return;
        }
        Line next = testLines.Find(targetIndex);
        if(next == null) {
            return;
        }
        lineObject.SetActive(true);      
        bool flag = lineObject.transform.Find("Illustration").GetComponent<IllustrationController>().
            ChangeTexture("Art/Scene1/" + next.illustration);
        flag = flag || lineObject.transform.Find("Illustration").GetComponent<RawImage>().color.a <= 0.01f;
        lineObject.transform.Find("Frame").GetComponent<FrameController>().ChangeText(next.dialogue, "Art/Scene1/" + next.frame, flag);
        lineObject.transform.Find("Frame").GetComponent<FrameController>().ChangeNameTag(next.name, flag);
        currentLine = next.lineIndex;
        nextLine = next.nextLine;
        lineLog.Add(nextLine);
    }

    //Automatically jump to the next line
    public void JumpToNext() {
        Debug.Log("Jumping into " + nextLine);
        ChangeLine(nextLine);
    }

    //Enable option based on targetIndex (same index in the optionJson)
    public void EnableOptions(string targetIndex) {
        nextBtn.SetActive(false);
        Option target = testOptions.Find(targetIndex);
        if(target == null) {
            Debug.Log("target option " + targetIndex + " not found.");
            return;
        }
        foreach(Button button in lineObject.transform.Find("Options").Find(target.optionLines.Length.ToString()).
            GetComponentsInChildren<Button>(true)){
            button.gameObject.SetActive(true);
            int optionIndex = button.gameObject.name[6] - 49;
            button.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = target.optionLines[optionIndex];
        }
        nextLine = "";
    }

    //After player made a choice, disable option buttons and move to the line that player chose
    //Show the illustration field in the option
    public void DisableOptions(int optionIndex) {
        nextBtn.SetActive(true);
        foreach(Button button in lineObject.transform.Find("Options").GetComponentsInChildren<Button>(true)){
            button.gameObject.SetActive(false);
        }
        Option target = testOptions.Find(currentLine);
        bool flag = lineObject.transform.Find("Illustration").GetComponent<IllustrationController>().
            ChangeTexture("Art/Scene1/" + target.illustration);
        lineObject.transform.Find("Frame").GetComponent<FrameController>().
            ChangeText(target.optionLines[optionIndex], "Art/Scene1/dialogue-box", flag);
        lineObject.transform.Find("Frame").GetComponent<FrameController>().
            ChangeNameTag("Meimei", flag);
        nextLine = target.nextLines[optionIndex];
        lineLog.Add(nextLine);
    }

    public bool CheckLog(string line) {
        return lineLog.Contains(line);
    }
}
