using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Options
{
    public Option[] options;

    //helper function to find the option with target index
    public Option Find(string index) {
        foreach(Option option in options) {
            if(option.optionIndex == index) {
                return option;
            }
        }
        return null;
    }
}