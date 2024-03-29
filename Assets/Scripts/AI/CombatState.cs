using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : StatePattern
{
    //We need the navemesh for this one as well
    private NavMeshAgent agent;
    //We need the player to calculate the angles and the distances
    private GameObject player;
    //All of these are going to be multiplied with random numbers to decide what the ai is going to do.
    //Default it to one so we avoid multiplying with 0
    private float agressionRate=1;
    private float blockRate=1;
    private float escapeRate=1;
    private float dodgeRate=1;

    //The outcome of the random number generation will be stored in this variable and this variable will be multiplied with the rates.
    private float agressionRandom;
    private float blockRandom;
    private float escapeRandom;
    private float dodgeRandom;
    //Since this needs an agent we gotta have the bool as well.
    private bool needsAgent;
    //Since we are seperating everything into functions it is better to have this global, causes less headache for me.
    private Vector3 direction;

    //Bool checks for all states. we do this so that the other states does not get interrupted by the other states.
    private bool inMovingToPlayerState;
    private bool inBlockingState;
    private bool inEscapeState;
    private bool inDodgeState;

    //TODO remove the random part of it rather then make it stages like he needs to get close to the player then punch him depending on what the player is doing or block or dodge

    //Going for random number state choosing might have been a bad idea I have to think of a better one.
    public override void EnterState(FiniteStateMachine finiteState)
    {
        Debug.Log("In chase");
        player=GameObject.FindGameObjectWithTag("Player");
        needsAgent=true;

        //Here we set the custom variables
        if(finiteState.GetComponent<AICustomVariables>())
        {
            agressionRate=finiteState.gameObject.GetComponent<AICustomVariables>().GetAgressionRate();
            blockRate=finiteState.gameObject.GetComponent<AICustomVariables>().GetBlockRate();
            escapeRate=finiteState.gameObject.GetComponent<AICustomVariables>().GetEscapeRate();
            dodgeRate=finiteState.gameObject.GetComponent<AICustomVariables>().GetDodgeRate();
        }
        
    }

    public override void UpdateState(FiniteStateMachine finiteState)
    {  
        //We get the direction rather then the distance so we can calculate the angle of the player.
        direction=player.transform.position-agent.transform.position;
        //This decides if the ai is going to move towards the player;
        MoveToPlayerState();
        //This makes the ai retreat
        EscapeState();
        DodgeState();

        //This makes it revert back to the idle if the player somehow gets out of combat
        if(!CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            finiteState.SwitchState(finiteState.idleState);
        }
    }

    public override void ExitState(FiniteStateMachine finite)
    {
    }

    public override void OnCollisionEnter(FiniteStateMachine finiteState)
    {
    }

    public override void SetNavMeshAgent(NavMeshAgent parentAgent)
    {
        //First we check if it needs an agent if not it returns back
        if(!needsAgent)
        {
            //Debug.Log("Does not need an agent");
            return;
        }
        agent=parentAgent;
    }

    private void MoveToPlayerState()
    {
        //First we get a random number and store it inside of random num
        //If one of these are true we want to stop calculating the random number so it does not interrupt the current playing state.
        if(!inMovingToPlayerState&&!inBlockingState&&!inEscapeState&&!inDodgeState)
        {
            //Lets try max wıth 5 rather then 10
            //If you do five it does not work rather then doing that just change the enterence minimum
            agressionRandom=Random.Range(0.0f,10.0f);
        }
        //Then we multiply it with the rate giving a higher chance to go into this state or a lower chance of going into it.
        agressionRandom*=agressionRate;
        //If the random num is equal or higher then the minimum threshold it goes into the state.
        if(agressionRandom>=5)
        {
            //Reset destination before setting it again to clear up the old one.
            agent.ResetPath();
            Debug.Log("In follow");
            //we set this to true so it does not get interrupted
            inMovingToPlayerState=true;
            //Reset every other random value to avoid conflicts
            //Maybe don't do this has weird results
            //escapeRandom=0;
            agent.SetDestination(player.transform.position);
            //When it reaches the player we reset it back to zero
            //OR rather then getting rondom num get agent.stoppingDistance
            if(direction.magnitude<=Random.Range(1,5))
            {
                Debug.Log("Out of follow");
                inMovingToPlayerState=false;
                //We might not need isstopped I WOULD KNEW IF MY COMPUTER DID NOT EXPLODE
            }
        }
    }

    private void EscapeState()
    {
        //If the distance is too high don't go into the escape state.
        if(direction.magnitude>=7)
        {
            Debug.Log("returned out of escape");
            return;
        }
        //Escape states is not really eascape but it is more of a retreat
        //Calculation for the direction vector
        Vector3 retreatDirection=agent.transform.position-player.transform.position;
        //If one of these are true we want to stop calculating the random number so it does not interrupt the current playing state.
        if(!inMovingToPlayerState&&!inBlockingState&&!inEscapeState&&!inDodgeState)
        {
            escapeRandom=Random.Range(0.0f,10.0f);
        }
        //Then we multiply it with the rate giving a higher chance to go into this state or a lower chance of going into it.
        escapeRandom*=escapeRate;
        if(escapeRandom>=8)
        {
            //Reset destination before setting it again to clear up the old one.
            agent.ResetPath();
            Debug.Log("in retreat");
            //We set this to true so we don't get interrupted by other states
            inEscapeState=true;
            //Reset every other random value to avoid conflicts
            //Maybe don't do this has weird results
            //agressionRandom=0;
            agent.SetDestination(retreatDirection);
            //We do a lookat so that the AI always looks at the player
            //We might want to do something smoother in the future
            agent.transform.LookAt(player.transform);
            //Randomly selects the distance it is going to go
            if(direction.magnitude>=Random.Range(1,5))
            {
                Debug.Log("Out of escape");
                inEscapeState=false;
            }
        }
    }


    //Maybe add if player is attacking here as well so they only dodge when you attack
    private void DodgeState()
    {
         //If one of these are true we want to stop calculating the random number so it does not interrupt the current playing state.
        if(!inMovingToPlayerState&&!inBlockingState&&!inEscapeState&&!inDodgeState)
        {
            dodgeRandom=Random.Range(0.0f,10.0f);
        }
        dodgeRandom*=dodgeRate;
        //Check if they have the dashsystem if not don't dash
        if(dodgeRandom>=5&&agent.gameObject.GetComponent<DashSystem>())
        {
            Debug.Log("In dash");
            Debug.Log(inDodgeState);
            inDodgeState=true;
            agent.gameObject.GetComponent<DashSystem>().StartCoroutine(agent.gameObject.GetComponent<DashSystem>().AIDash());
            inDodgeState=false;
        }
    }
}