using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButtons : MonoBehaviour
{
    public Button chapter1;
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
        if(PlayerPrefs.HasKey("1")) {
            if(PlayerPrefs.HasKey("2")) {
                chapter2.GetComponent<Outline>().effectDistance = new Vector2(0f, 0f);
                chapter3.GetComponent<Outline>().effectDistance = new Vector2(4f, 4f);
            }
            else {
                chapter3.GetComponent<Outline>().effectDistance = new Vector2(0f, 0f);
                chapter2.GetComponent<Outline>().effectDistance = new Vector2(4f, 4f);
            }
            chapter1.GetComponent<Outline>().effectDistance = new Vector2(0f, 0f);
        }
        else{
            chapter1.GetComponent<Outline>().effectDistance = new Vector2(4f, 4f);
            chapter2.GetComponent<Outline>().effectDistance = new Vector2(0f, 0f);
            chapter3.GetComponent<Outline>().effectDistance = new Vector2(0f, 0f);
        }
    }
}
