using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   public static UIManager instance;
    [SerializeField] private Image healthFillBar;
    //We might need it in the future
    [SerializeField] private GameObject player;
    //This is the amount of lerp the bar is going to do
    [SerializeField] private float lerpAmount=3.0f;
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
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

    //This function makes it so that we lerp through the fill values
    private float BarLerp()
    {
        lerpSpeed=lerpAmount*Time.deltaTime;
        return lerpSpeed;
    }
}
