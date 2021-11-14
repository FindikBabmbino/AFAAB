using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]private float dialogueSpeed=0;

    //This holds the ink json that is specific to this character
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    //This will feed the information into the dialogue box.
    public void TriggerDialogue()
    {
        ChatterBoxManager.instance.StartDialogue(inkJSON,dialogueSpeed);
    }
}