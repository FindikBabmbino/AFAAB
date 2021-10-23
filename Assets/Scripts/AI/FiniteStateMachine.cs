using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This is the finite state machine that will use every state
public class FiniteStateMachine : MonoBehaviour
{
    //We will get the current state first to see what state the ai is in
    private StatePattern currentState;
    //We have to instantiate every state we wrote
    public StatePattern idleState=new GenericIdle();
    public StatePattern combatState=new CombatState();

    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        //Get the agent it is safer to get it in the awake function.
        agent=GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        //Start state of the state machine
        currentState=idleState;
        currentState.EnterState(this);
        currentState.SetNavMeshAgent(agent);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void SwitchState(StatePattern statePattern)
    {
        //This will be called from the states to change the state after the condition is met 
        currentState=statePattern;
        statePattern.EnterState(this);
        statePattern.SetNavMeshAgent(agent);
    }
}