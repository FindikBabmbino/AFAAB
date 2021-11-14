using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySystem : MonoBehaviour
{
    [Header("Fly stats")]
    [SerializeField] private float maxFlyMeter=100.0f;
    //Don't really play around with this rather play around with the maxfly to determine how much fly meter Sarp can have
    [SerializeField] private float currentFlyMeter;



    private void Update()
    {
        //This calculate fly meter by dividing current with max
        UIManager.instance.ModifyFlyAmount(currentFlyMeter/maxFlyMeter);
    }
    //This will be called from attack script to increase fly meter dependening on how much is passed into the function
    public void IncreaseFlyAmount(float var)
    {
        //If the currentflymeter is higher or at the same level as the maxfly just return out
        if(currentFlyMeter>=maxFlyMeter)
        {
            return;
        }
        //Clamp the meter so it does not go higher then maxFlyMeter
        Mathf.Clamp(currentFlyMeter,0,maxFlyMeter);
        //Then add the variable
        currentFlyMeter+=var;

    }
    //This will be used from the attack script again but this time it will decrease hit depending on how much is passed on
    public void DecreaseFlyAmount(float var)
    {
        if(currentFlyMeter<=0)
        {
            return;
        }  
        //Just do the same stuff from the increase function to avoid going above zero
        Mathf.Clamp(currentFlyMeter,0,maxFlyMeter);
        currentFlyMeter-=var;
    }
    //This will be called from the ui to see how much of the bar has to be filled.
    public float GetFlyAmount()
    {
        return currentFlyMeter;
    }

    //These will be called from the item generic
    public float ReturnCurrentFlyEnergy()
    {
        return currentFlyMeter;
    }
    public float ReturnMaxFlyEnergy()
    {
        return maxFlyMeter;
    }
}