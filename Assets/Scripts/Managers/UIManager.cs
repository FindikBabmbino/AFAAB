using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{   public static UIManager instance;
    [SerializeField] private Image healthFillBar;
    [SerializeField] private Image flyFillBar;
    //We might need it in the future
    [SerializeField] private GameObject player;
    //This is the amount of lerp the bar is going to do
    [SerializeField] private float lerpAmount=3.0f;
    //This is the panel where the dialogue is displayed and we will close and open this depending on if there is a dialogue
    [SerializeField] private GameObject dialoguePanel;
    [Header("Choice Panel")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choiceText;
    //This will be used in the barlerp to store the lerp speed so that we can use it in the mathf.lerp
    private float lerpSpeed;
    //If we get a choice set this to true so we can't accidentally end the dialogue when we press e or the intreact button
    private bool isMakingAChoice;
    private void Awake()
    {
        //Basic singelton setup
        if(instance==null)
        {
            instance=this;
        }
        if(instance!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //We want to close the dialogue box at the start to be sure that it is closed
        if(dialoguePanel!=null)
        {
            dialoguePanel.SetActive(false);
        }

        //Since the number of choice game objects and choice text objects need to be same we set it to the same lenght at the start
        choiceText=new TextMeshProUGUI[choices.Length];
        int index=0;
        foreach(GameObject choice in choices)
        {
            choiceText[index]=choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ModifyHealthAmount(float var)
    {
        //Lerps between the fillamount and the current health value.
        healthFillBar.fillAmount=Mathf.Lerp(healthFillBar.fillAmount,var,BarLerp());
    }
    //This calculates the fly bat
    public void ModifyFlyAmount(float var)
    {
        //Lerps between the fillamount and the current health value.
        flyFillBar.fillAmount=Mathf.Lerp(flyFillBar.fillAmount,var,BarLerp());
    }

    //This will open the dialogue box
    public void OpenDialogueBox()
    {
        dialoguePanel.SetActive(true);
    }

    //This will close it
    public void CloseDialogueBox()
    {
        dialoguePanel.SetActive(false);
    }
    

    //This function makes it so that we lerp through the fill values
    private float BarLerp()
    {
        lerpSpeed=lerpAmount*Time.deltaTime;
        return lerpSpeed;
    }

    //This sets the correct choice names from the story
    public void DisplayChoices()
    {
        List<Choice> currentChoices=ChatterBoxManager.instance.ReturnCurrentStory().currentChoices;

        //If the lenght of the list is bigger then the available gameobjects give error
        if(currentChoices.Count>choices.Length)
        {
            Debug.LogWarning("More choices then there are being allowed to use");
        }

        int index=0;
        //Enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choiceText[index].text=choice.text;
            index++;
        }
        //Go through remaining choices the UI supports and make sure they are hidden
        //Set i as the index so we dont repeat old choices.
        for(int i=index;i<choices.Length;i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        //If the index remains zero it means that there are no choices 
        if(index!=0)
        {
            isMakingAChoice=true;
        }
        StartCoroutine(SelectFirstChoice());
    }

    //This will make it so that we can make a choice and the story can continue
    //And since we keep then in an array it is easier to reference this in the inspector on each button
    public void MakeChoice(int choiceIndex)
    {
        ChatterBoxManager.instance.ReturnCurrentStory().ChooseChoiceIndex(choiceIndex);
        ChatterBoxManager.instance.ContinueStory();
        isMakingAChoice=false;
    }

    //This sets the first selected choice
    private IEnumerator SelectFirstChoice()
    {
        //Event system requires we clear it first then wait
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    //This will make it so that we can hide it when we are typing the letter
    public void HideChoices()
    {
        foreach(GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }

    public bool ReturnisMakingAChoice()
    {
        return isMakingAChoice;
    }
}