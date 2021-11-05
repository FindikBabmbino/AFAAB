using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ChatterBoxManager : MonoBehaviour
{
    public static ChatterBoxManager instance;
    [Header("UI stuff")]
    [SerializeField] private TextMeshProUGUI talkerName;
    [SerializeField] private TextMeshProUGUI chatterBoxText;
    //Because of FIFO(first in first out) we want to use queues
    private Queue<string> sentences;
    //This bool will keep track if the player is in a dialogue
    private bool playerIsInDialogue;

    //We get the input so we can skip continue through the dialogue
    private PlayerInput playerInput;

    //This float controlls the speed of text
    private float textSpeed;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else if(instance!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        //This gets the player input
        playerInput=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }
    private void Start()
    {
        //Initiliase the queue
        sentences=new Queue<string>();
    }
    
    //This will be called from the dialogue trigger so we can display the dialogue
    public void StartDialogue(Dialogue dialogue,float textSpeed=0)
    {  
        //Set this to true
        playerIsInDialogue=true;
        //When starting the dialogue we have to open the panel so we can see what is being displayed.
        UIManager.instance.OpenDialogueBox();
        //This sets the characters name
        talkerName.text=dialogue.ReturnName();
        //This is to make sure that there is no leftover dialogues in the sentences string
        sentences.Clear();

        //This foreach will put each sentence in dialogue to que.
        foreach(string sentence in dialogue.GetSentences())
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    //Like the name suggest this will display the next sentence
    public void DisplayNextSentence()
    {
        //If there are no sentences left call end dialogue end return out of it
        if(sentences.Count==0)
        {
            EndDialogue();
            return;
        }

        string nextSentence=sentences.Dequeue();
        //If the typing is allready running but the player skips we want to stop all coroutines so we can start a new one
        StopAllCoroutines();
        //This sets the text on the box
        StartCoroutine(TypeSentence(nextSentence));
    }

    //This will be called when there is no more dialogues left
    private void EndDialogue()
    {
        //Since we reached the end we need to set this to false
        playerIsInDialogue=false;
        //Since the dialogue ends we want to call this function
        UIManager.instance.CloseDialogueBox();
    }

    //This will type out the letters in the chatterbox
    private IEnumerator TypeSentence(string sentence)
    {
        //Start out with a null text so we can type something in and is initiated
        chatterBoxText.text="";
        //This will loop through the string and get the letters
        foreach(char letter in sentence.ToCharArray())
        {
            chatterBoxText.text+=letter;
            //This controlls the speed of the text
            yield return new WaitForSeconds(textSpeed);
            yield return null;
        }
    }

    //Since we don't want the player to do some stuff while he is talking we want to do this.
    public bool ReturnPlayerIsInDialogue()
    {
        return playerIsInDialogue;
    }

    //We will use this to change the text speed in diffrent scripts or let us change it through events
    public void ChangeTextSpeed(int var)
    {
        textSpeed=var;
    }
}