using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class ChatterBoxManager : MonoBehaviour
{
    public static ChatterBoxManager instance;
    [Header("UI stuff")]
    [SerializeField] private TextMeshProUGUI talkerName;
    [SerializeField] private TextMeshProUGUI chatterBoxText;

    private Story currentStory;
    //This bool will keep track if the player is in a dialogue
    private bool playerIsInDialogue;

    //We get the input so we can skip continue through the dialogue
    private PlayerInput playerInput;

    //This float controlls the speed of text
    private float textSpeed;

    //This will be true when the typewritter coroutine starts
    public bool isWrittingText;

    //These const will hold the tags from ink
    private const string SPEAKER_TAG="speaker";
    private const string ANIMATION_TAG="animation";

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
    
    //This will be called from the dialogue trigger so we can display the dialogue
    public void StartDialogue(TextAsset inkJSON,float textSpeed=0)
    {  
        //Set the story to this
        currentStory=new Story(inkJSON.text);
        //Set this to true
        playerIsInDialogue=true;
        //When starting the dialogue we have to open the panel so we can see what is being displayed.
        UIManager.instance.OpenDialogueBox();
        //Get the choices
        UIManager.instance.DisplayChoices();
        //Check if the story can continue
        ContinueStory();

    }

    //Like the name suggest this will display the next sentence
    public void ContinueStory()
    {
        if(currentStory.canContinue&&!isWrittingText)
        {
            //Set the text to current story
            string texToHold=currentStory.Continue();
            StartCoroutine(TypeSentence(texToHold));
            //Display choices if any for this dialogue line
            UIManager.instance.DisplayChoices();
            //handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            EndDialogue();
        }
        
    }

    private void HandleTags(List<string> currentTags)
    {
        //Loop through each tag and handle it accordingly
        foreach(string tag in currentTags)
        {
            //Parse the tags
            string[] splitTag=tag.Split(':');
            if(splitTag.Length!=2)
            {
                Debug.LogError("Tag could not be appopriatly parsed:" +tag);
            }
            string tagKey=splitTag[0].Trim();
            string tagValue=splitTag[1].Trim();

            //Handle the tag
            //TODO Add the animation states when we have them 
            switch(tagKey)
            {
                case SPEAKER_TAG:
                    talkerName.text=tagValue;
                    break;
                case ANIMATION_TAG:
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: "+ tag);
                    break;
            }
        }
    }


    //This will be called when there is no more dialogues left
    private void EndDialogue()
    {
         Debug.Log("in end dialogue");
        //Since we reached the end we need to set this to false
        playerIsInDialogue=false;
        //Since the dialogue ends we want to call this function
        UIManager.instance.CloseDialogueBox();
        //Reset the text
        chatterBoxText.text="";
    }

    //This will type out the letters in the chatterbox
    private IEnumerator TypeSentence(string sentence)
    {
        isWrittingText=true;
        //Start out with a null text so we can type something in and is initiated
        chatterBoxText.text="";
        //This will loop through the string and get the letters
        foreach(char letter in sentence.ToCharArray())
        {
            isWrittingText=true;
            chatterBoxText.text+=letter;
            //This controlls the speed of the text
            yield return new WaitForSeconds(textSpeed);
            yield return null;
            isWrittingText=false;
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

    //This is so we can call the current story from other scripts and get some functions
    public Story ReturnCurrentStory()
    {
        return currentStory;
    }

    //TODO-Figure this out 
    //This will be called from initiate dialouge if the couroutine is being run this will start and skip the couroutine
    public void SkipWrittingText(InputAction.CallbackContext context)
    {
      StartCoroutine(SkipWritting());
    }

    private IEnumerator SkipWritting()
    {
        //If the player presses the key to continue the story again while it is being written it will skip the animation
        if(isWrittingText)
        {
            chatterBoxText.text="";
            chatterBoxText.text=currentStory.currentText;
        }
        else
        {
            yield return null;
        }
        yield return null;
    }
}