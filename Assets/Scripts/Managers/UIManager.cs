using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //This will be used in the barlerp to store the lerp speed so that we can use it in the mathf.lerp
    private float lerpSpeed;
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
}
