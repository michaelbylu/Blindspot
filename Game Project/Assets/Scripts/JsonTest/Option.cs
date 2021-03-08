using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Option
{
    //The string texts to be shown on options
    public string[] optionLines;
    //The lines after each option (see line.cs for more details)
    public string[] nextLines;
    //String index for this option
    public string optionIndex;
    //The name of illustration texture, if left as empty string then nothing happens
    // (MUST be in the Resources folder)
    public string illustration;
}
