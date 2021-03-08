using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Lines
{
    public Line[] lines;

    //helper function to find the line with target index
    public Line Find(string index) {
        foreach(Line line in lines) {
            if(line.lineIndex == index) {
                return line;
            }
        }
        return null;
    }
}
