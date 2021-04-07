using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using System;

public class Logger : MonoBehaviour
{
    public string associatedSheet = "1hnW_3tQoynfaq0_L1UizJoBzl7V_XgiKG5FV5OwbMws";
    public string associatedWorksheet = "Stats";
    private string column;
    private string value;
    private string index;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Index")) {
            SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), GetIndex);
        }
        else {
            index = PlayerPrefs.GetString("Index");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogData(string columnId, string content) {
        column = columnId;
        value = content;
        SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), UpdateData);
    }

    private void UpdateData(GstuSpreadSheet ss) {
        ss[index, column].UpdateCellValue(associatedSheet, associatedWorksheet, value);
    }

    public void GetIndex(GstuSpreadSheet ss) {
        int count =  Int32.Parse(ss["A1"].value) + 2;
        string row = "A" + count.ToString();
        string index = System.DateTime.UtcNow.ToString();
        string dateTime = System.DateTime.UtcNow.ToString();
        ss["A1"].UpdateCellValue(associatedSheet, associatedWorksheet, (count - 1).ToString());
        List<string> list1 = new List<string>()
        {
            index
        };
        for(int i = 0; i < 10; i++) {
            list1.Add("null");
        }
        SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet, row), 
            new ValueRange(list1), null);
        PlayerPrefs.SetString("Index", index);
    }
}
