using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Line
{
    //The string dialogue to be shown in the text box
    public string dialogue;
    //The name of illustration texture (MUST be in the Resources folder)
    public string illustration;
    //The name of frame texture (MUST be in the Resources folder)
    public string frame;
    //String index for this line
    public string lineIndex;
    //String index for the next line
    public string nextLine;
    //The name tag of each character
    public string name;
}
