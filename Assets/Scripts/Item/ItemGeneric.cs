using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    //This is just a copy of the scriptable one this is probably going to work the best as a monobehaviour
    //Unique name for the item
    [SerializeField]private string itemName;
    //Description of the itme
    [SerializeField]private string itemDescription;
    //Health To Regain
    [SerializeField]private int amountOfHealth;
    //Fly to energy to regain
    [SerializeField]private int FlyEnergyAmount;
    //Set the type of consumable
    [SerializeField]private bool isHealthItem;
    [SerializeField]private bool isFlyItem;
    //Set if it is wearable
    [SerializeField]private bool isWearable;
    //Set armor protection
    [SerializeField]private int protection;
    //Image of the item
    [SerializeField]private Sprite image;

    //This will return true when it is used and if it is true the item will be deleted out of the inventory
    private bool isUsed;


    //These will return the variables
    public string ReturnItemName()
    {
        return itemName;
    }
    public string ReturnDescription()
    {
        return itemDescription;
    }
    public int ReturnHealthAmount()
    {
        return amountOfHealth;
    }
    public int ReturnFlyEnergyAmount()
    {
        return FlyEnergyAmount;
    }
    public bool ReturnIsHealthItem()
    {
        return isHealthItem;
    }
    public bool ReturnIsFlyItem()
    {
        return isFlyItem;
    }
    public bool ReturnIsWearable()
    {
        return isWearable;
    }
    public int ReturnProtection()
    {
        return protection;
    }
    public Sprite ReturnSprite()
    {
        return image;
    }

    public void ChangeItemName(string name)
    {
        itemName=name;
    }

    public void ChangeDescription(string description)
    {
        itemDescription=description;
    }
    public void ChangeHealthAmount(int amount)
    {
        amountOfHealth=amount;
    }
    public void ChangeFlyEnergyAmount(int amount)
    {
        FlyEnergyAmount=amount;
    }
    public void ChangeIsHealthItem(bool var)
    {
        isHealthItem=var;
    }
    public void ChangeIsFlyItem(bool var)
    {
        isFlyItem=var;
    }
    public void ChangeIsWearable(bool wearable)
    {
        isWearable=wearable;
    }
    public void ChangeProtectionRate(int amount)
    {
        protection=amount;
    }
    public void ChangeSprite(Sprite sprite)
    {
        image=sprite;
    }


    //This will handle the ussage of the item
    public void UseItem()
    {
        Debug.Log("is in use item");
        //If it is the consumable get the guy using it 
        if(isHealthItem||isFlyItem)
        {
            GameObject healthObject=GameObject.FindWithTag("Player");
            //It is a bit messy but it works I don't think i really have to explain the logic since it is pretty simple 
            if(isHealthItem)
            {
                //If the health or the fly meter is full is full don't use the item
                if(healthObject.GetComponent<HealthSystem>().ReturnCurrentHealth()>=healthObject.GetComponent<HealthSystem>().ReturnMaxHealth())
                {   
                }
                else
                {
                    healthObject.GetComponent<HealthSystem>().Heal(amountOfHealth);
                    isUsed=true;
                }
            }
            if(isFlyItem)
            {
                //If the health or the fly meter is full is full don't use the item
                if(healthObject.GetComponent<FlySystem>().ReturnCurrentFlyEnergy()>=healthObject.GetComponent<FlySystem>().ReturnMaxFlyEnergy())
                {   
                }
                else
                {
                    healthObject.GetComponent<FlySystem>().IncreaseFlyAmount(FlyEnergyAmount);
                    isUsed=true;
                }
            }
        }
    }
    //This will be called from the manager to see if the item gets used if it gets used it will be deleted
    public bool ReturnIsUsed()
    {
        return isUsed;
    }
}