using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //This is so we can access the variables inside it.
    [SerializeField]private Dialogue dialogue;
    [SerializeField]private float dialogueSpeed=0;

    //This will feed the information into the dialogue box.
    public void TriggerDialogue()
    {
        ChatterBoxManager.instance.StartDialogue(dialogue,dialogueSpeed);
    }
}