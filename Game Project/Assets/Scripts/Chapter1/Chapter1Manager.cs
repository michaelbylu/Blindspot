using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class Chapter1Manager : MonoBehaviour
{
    [System.Serializable]
    public struct part {
        public GameObject[] lines;
    }
    public part[] parts;
    private int partIndex = 0;
    private int lineIndex = 0;
    void Start()
    {
        
    }
    private void OnEnable() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JumpTo(int line) {
        GameObject target = parts[partIndex].lines[line];
        if(target == null) {
            return;
        }
        parts[partIndex].lines[lineIndex].SetActive(false);
        target.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = (target.GetComponentInChildren<Button>() == null);
    }

    public void JumpToNext() {
        parts[partIndex].lines[lineIndex].SetActive(false);
        if(lineIndex == parts[partIndex].lines.Length - 1) {
            partIndex++;
            lineIndex = 0;
        }
        else {
            lineIndex++;
        }
        GameObject nextLine = parts[partIndex].lines[lineIndex];
        nextLine.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = (nextLine.GetComponentInChildren<Button>() == null);
    }
    public void HideCurrentLine() {
        GameObject nextLine = parts[partIndex].lines[lineIndex];
        nextLine.SetActive(false);
    }

    private void OnMouseUp() {
        JumpToNext();
    }
}
