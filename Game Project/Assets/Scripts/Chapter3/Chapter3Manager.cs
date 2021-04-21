using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Manager : MonoBehaviour
{
    private string currentDifficulty;
    private int currentStage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckStage() {

    }

    public void ChangeStage()
    {
        currentStage++;
        CheckStage();
    }

    public void ChangeStage(string puzzleDifficulty) {
        currentStage++;
        currentDifficulty = puzzleDifficulty;
        CheckStage();
    }
}
