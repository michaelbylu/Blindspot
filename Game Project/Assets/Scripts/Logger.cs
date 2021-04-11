using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Logger : MonoBehaviour
{
    public string formUrl = "1hnW_3tQoynfaq0_L1UizJoBzl7V_XgiKG5FV5OwbMws";
    public string[] fields;   
    public string[] entrys;
    private Dictionary<string, string> map;
    private string id;
    private bool submit = true;
    // Start is called before the first frame update
    void Start()
    {
        map = new Dictionary<string, string>();
        map.Add("Id", DateTime.UtcNow.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.E) && submit) {
            submit = false;
        }
    }

    IEnumerator Post() {
        WWWForm form = new WWWForm();
        foreach(var pair in map) {
            form.AddField("entry." + entrys[Array.IndexOf(fields, pair.Key)], pair.Value);
        }
        byte[] rawData = form.data;
        UnityWebRequest www = UnityWebRequest.Post(formUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            www.Dispose();
            Debug.Log("Form upload complete!");
        }
        
    }
    public void LogData(string key, string value) {
        map.Add(key, value);
    }

    public void SendLogs() {
        if(!submit) {
            return;
        }
        StartCoroutine(Post());
    }
}
