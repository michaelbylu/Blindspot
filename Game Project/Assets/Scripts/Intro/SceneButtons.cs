using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButtons : MonoBehaviour
{
    public Button chapter2;
    public Button chapter3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            SceneManager.LoadScene(2);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(3);
        }
        chapter2.interactable = PlayerPrefs.HasKey("1");
        chapter3.interactable = PlayerPrefs.HasKey("2");
    }
}
