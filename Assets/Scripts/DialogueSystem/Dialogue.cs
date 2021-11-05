using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will communicate with the chatterbox system so we can put in dialigue.
[System.Serializable]
public class Dialogue
{
    [SerializeField]private string name;
    [TextArea(3,10)]
    [SerializeField]private string[] sentences;

    public string[] GetSentences()
    {
        return sentences;
    }

    public string ReturnName()
    {
        return name;
    }
}
