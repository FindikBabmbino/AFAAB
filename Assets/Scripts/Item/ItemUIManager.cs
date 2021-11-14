using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager instance;
    //This will hold the item the player buys and this will be put inside the list
    [SerializeField]private ItemGeneric itemToHold;
    [SerializeField]private List<ItemGeneric> Items=new List<ItemGeneric>();
    [SerializeField]private Button[] buttons;
    [SerializeField]private GameObject conformationPanel;
    [SerializeField]private Button yesButton,noButton;
    [SerializeField]private TextMeshProUGUI itemName;
    [SerializeField]private TextMeshProUGUI itemDescription;

    //This will hold the index for the last pressed buttons index
    private int indexOfLastPressedButton;
    //This will keep track of the click count
    private int clickCount;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
            //We want this to persist through the game
        }
        else if(instance!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //set this to disable at start just to be safe
        if(conformationPanel!=null)
        {
            conformationPanel.SetActive(false);
        }
        //This one is only for test
        PutItemInEmptySlot();
        //Go through all of them and add them a listener
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(DisplayItemInformation);
        }
        yesButton.onClick.AddListener(UseItem);
        noButton.onClick.AddListener(CloseConformation);
    }

    //This will decide how the item will be used
    private void DisplayItemInformation()
    {
        //This will be compared with the top if check
        int index=indexOfLastPressedButton;
        //In every click increament this
        clickCount++;
        //If the player clicks on another button reset the click count to one
        if(indexOfLastPressedButton!=index)
        {
            clickCount=0;
        }
        if(buttons[indexOfLastPressedButton].GetComponentInChildren<ItemGeneric>()==null)
        {
            itemName.text="";
            itemDescription.text="";
            Debug.Log("No Items");
            clickCount=0;
            return;
        }
        //when we have the child we are going to access its contents and get the coolio stuff
       itemName.text=Items[indexOfLastPressedButton].GetComponent<ItemGeneric>().ReturnItemName();
       itemDescription.text=Items[indexOfLastPressedButton].GetComponent<ItemGeneric>().ReturnDescription();
       //If the click count is equal to 2 or goes higher then 2 open the conformation window
       if(clickCount>=2)
       {
           OpenConformation();
       }
    }

    private void UseItem()
    {
        Items[indexOfLastPressedButton].GetComponent<ItemGeneric>().UseItem();
        //After the item is used discard the item
       if(Items[indexOfLastPressedButton].GetComponent<ItemGeneric>().ReturnIsUsed())
       {
            Destroy(Items[indexOfLastPressedButton].gameObject); 
            Items.RemoveAt(indexOfLastPressedButton);
            //reset click count
            clickCount=0;
            //Also close the conformation window
            CloseConformation();
            //Reset the strings 
            itemName.text="";
            itemDescription.text="";
       }

    }

    //This will be referenced inside of the editor and when we press it is going to send back it's index number
    public void ReturnButtonIndexNum(int indexNum)
    {
        indexOfLastPressedButton=indexNum;
    }

    //This will be called when we first get an item
    public void PutItemInEmptySlot()
    {
        for(int i=0; i<buttons.Length;i++)
        {
            //Since we want to add the item to the next empty box after one loop we will return
            if(buttons[i].gameObject.transform.childCount<=0)
            {
                GameObject imageToChild=new GameObject();
                imageToChild.transform.SetParent(buttons[i].gameObject.transform);
                imageToChild.transform.localPosition=new Vector3(0,0,0);
                imageToChild.AddComponent<Image>();
                //Now we will add the item generic component and move every data from the itemToHold.
                ChangeItemGenericValues(imageToChild);
                //Change the sprite and the text
                if(imageToChild.GetComponent<ItemGeneric>().ReturnSprite()!=null)
                {
                    imageToChild.GetComponent<Image>().sprite=imageToChild.GetComponent<ItemGeneric>().ReturnSprite();
                }
                //And add it to the list
                Items.Add(imageToChild.GetComponent<ItemGeneric>());
                return;
            }
        }
    }

    //This will be called from the item generic script
    public void PutItemInToHold(ItemGeneric generic)
    {
        itemToHold=generic;
    }
    
    //Now we will add the item generic component and move every data from the itemToHold.
    private void ChangeItemGenericValues(GameObject gameObject)
    {
        gameObject.AddComponent<ItemGeneric>();
        gameObject.GetComponent<ItemGeneric>().ChangeItemName(itemToHold.ReturnItemName());
        gameObject.GetComponent<ItemGeneric>().ChangeDescription(itemToHold.ReturnDescription());
        gameObject.GetComponent<ItemGeneric>().ChangeHealthAmount(itemToHold.ReturnHealthAmount());
        gameObject.GetComponent<ItemGeneric>().ChangeFlyEnergyAmount(itemToHold.ReturnFlyEnergyAmount());
        gameObject.GetComponent<ItemGeneric>().ChangeIsHealthItem(itemToHold.ReturnIsHealthItem());
        gameObject.GetComponent<ItemGeneric>().ChangeIsFlyItem(itemToHold.ReturnIsFlyItem());
        gameObject.GetComponent<ItemGeneric>().ChangeIsWearable(itemToHold.ReturnIsWearable());
        gameObject.GetComponent<ItemGeneric>().ChangeProtectionRate(itemToHold.ReturnProtection());
        gameObject.GetComponent<ItemGeneric>().ChangeSprite(itemToHold.ReturnSprite());
    }

    //This will open the conformation screen
    private void OpenConformation()
    {
        conformationPanel.SetActive(true);
    }
    //This will close the conformation screen
    private void CloseConformation()
    {
        conformationPanel.SetActive(false);
        //Reset click count
        clickCount=0;
    }
}