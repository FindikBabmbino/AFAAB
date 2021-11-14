using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Item",menuName ="Inventory Items")]
public class InventoryItem : ScriptableObject
{
    //Unique name for the item
    public string ItemName;
    //Description of the itme
    public string ItemDescription;
    //Health To Regain
    public int amountOfHealth;
    //Set if it is consumable
    public bool isConsumable;
    //Set if it is wearable
    public bool isWearable;
    //Set armor protection
    public int protection;
    //Image of the item
    public Sprite image;

}
