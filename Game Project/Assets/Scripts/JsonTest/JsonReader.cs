using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonReader : MonoBehaviour
{
    public TextAsset linesJson;
    public TextAsset optionsJson;
    public GameObject lineObject;
    private string currentLine = "0";
    private string nextLine = "";
    private Lines testLines;
    private Options testOptions;
    // Start is called before the first frame update
    void Start()
    {
        testLines = JsonUtility.FromJson<Lines>(linesJson.text);
        testOptions = JsonUtility.FromJson<Options>(optionsJson.text);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
            ChangeLine("0");
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
            ChangeLine("9");
        }
    }

    //Switch dialogue content based on targetIndex
    //When targetIndex indicate options, turn on option buttons
    public void ChangeLine(string targetIndex) {
        if(targetIndex == "puzzle1") {
            lineObject.SetActive(false);
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
        lineObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = next.dialogue;
        lineObject.transform.Find("Frame").GetComponent<RawImage>().texture = 
            Resources.Load<Texture2D>("Art/Scene1/text_box");
        if(next.image == "") {
            lineObject.transform.Find("Illustration").GetComponent<RawImage>().texture = null;
        }
        else {
            lineObject.transform.Find("Illustration").GetComponent<RawImage>().texture = 
            Resources.Load<Texture2D>("Art/Scene1/" + next.image);
        }
        currentLine = next.lineIndex;
        nextLine = next.nextLine;
    }

    //Automatically jump to the next line
    public void JumpToNext() {
        Debug.Log("Jumping into " + nextLine);
        ChangeLine(nextLine);
    }

    //Enable option based on targetIndex (same index in the optionJson)
    public void EnableOptions(string targetIndex) {
        Option target = testOptions.Find(targetIndex);
        if(target == null) {
            Debug.Log("target option " + targetIndex + " not found.");
            return;
        }
        Debug.Log("getting child buttons");
        foreach(Button button in lineObject.transform.Find("Options").GetComponentsInChildren<Button>(true)){
            button.gameObject.SetActive(true);
            int optionIndex = button.gameObject.name[6] - 49;
            button.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = target.optionLines[optionIndex];
            Debug.Log("option button " + optionIndex + " enabled!");
        }
        nextLine = "";
    }

    //After player made a choice, disable option buttons and move to next line
    public void DisableOptions(int optionIndex) {
        foreach(Button button in lineObject.transform.Find("Options").GetComponentsInChildren<Button>(true)){
            button.gameObject.SetActive(false);
        }
        Option target = testOptions.Find(currentLine);
        ChangeLine(target.nextLines[optionIndex]);
    }
}
