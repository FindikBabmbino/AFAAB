using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitiateDialogue : MonoBehaviour
{
   //This script will be used to initiate dialogue with intreactable npcs
   [SerializeField]private List<DialogueTrigger> npcToTalkTo=new List<DialogueTrigger>();
   //This will control the minimum distance the player needs to be talk to somebody
   [SerializeField]private float minDistanceToTalk=3;
    //Since we are going to use the intreaction key to open the dialogue box
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput=GetComponent<PlayerInput>();
    }
   private void Start()
   {
       //We will only put the npcs that have a dialogue box in them
       npcToTalkTo.Add(GameObject.FindObjectOfType<DialogueTrigger>());
   }

    //Sub on enable
    private void OnEnable()
    {
        playerInput.actions["Interact"].performed+=CalculateDistance;
    }
    //Unsub on disable to avoid memory leak
    private void OnDisable()
    {
        playerInput.actions["Interact"].performed-=CalculateDistance;
    }

    private void CallTriggerDialogue(int numOfList)
    {
        //If the player is in a battle don't start a dialogue
        if(CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            return;
        }
        if(!ChatterBoxManager.instance.ReturnPlayerIsInDialogue())
        {
             npcToTalkTo[numOfList].TriggerDialogue();
        }
    }

    //This will calculate the closest object that the player can talk to and you can talk to that guy person
    private void CalculateDistance(InputAction.CallbackContext context)
    {
        Debug.Log("In calculate distance");
        foreach(var obj in npcToTalkTo)
        {
            //This i keeps track of which one of them it is then it is passed into call trigger dialogue.
            int i=0;
                     //We get the distance between each npc
            Vector3 distance=transform.position-obj.gameObject.transform.position;
            float distanceSqrd=distance.sqrMagnitude;
            //If the distance matches the min distance we call the call trigger dialogue.
            if(distanceSqrd<=minDistanceToTalk&&!ChatterBoxManager.instance.ReturnPlayerIsInDialogue())
            {
                minDistanceToTalk=distanceSqrd;
                Debug.Log(i);
                CallTriggerDialogue(i);
            }
            //If the player is already in a dialogue just continue the dialogue
            //If there is a choice we don't want the player to press e 
            if(ChatterBoxManager.instance.ReturnPlayerIsInDialogue()&&!UIManager.instance.ReturnisMakingAChoice())
            {
                Debug.Log("In else if check");
                ChatterBoxManager.instance.ContinueStory();
            }
            //This is so that it does not crash the game if it finds nothing
            else
            {
                return;
            }
            i++;
        }
    }
}