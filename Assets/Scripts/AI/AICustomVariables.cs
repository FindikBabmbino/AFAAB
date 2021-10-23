using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICustomVariables : MonoBehaviour
{
    //This script will be used to set custom agression rate,block rate, dodge rate and then will be called from the combat state script.
    [SerializeField]private float aggresionRate;
    [SerializeField]private float blockRate;
    [SerializeField]private float escapeRate;
    [SerializeField]private float dodgeRate;

    //All of these functions will be called from the combat state to replace the variables there.
    public float GetAgressionRate()
    {
        return aggresionRate;
    }
    public float GetBlockRate()
    {
        return blockRate;
    }
    public float GetEscapeRate()
    {
        return escapeRate;
    }
    public float GetDodgeRate()
    {
        return dodgeRate;
    }

    //These will be called from other scripts to edit them through script.
    public void SetAggressionRate(float var)
    {
        aggresionRate=var;
    }
    public void GetBlockRate(float var)
    {
        blockRate=var;
    }
    public void GetEscapeRate(float var)
    {
        escapeRate=var;
    }
    public void GetDodgeRate(float var)
    {
        dodgeRate=var;
    }
}