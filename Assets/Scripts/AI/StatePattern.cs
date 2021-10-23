using UnityEngine;
using UnityEngine.AI;

public abstract class StatePattern
{
    //This is the abstract class that wil be used by every state
    //Since these are abstract we need to define them in every chid
    //We pass on the statemachine because that is the thing that is going to send data in to the state
   public abstract void EnterState(FiniteStateMachine finiteState);
   public abstract void UpdateState(FiniteStateMachine finiteState);
   public abstract void ExitState(FiniteStateMachine finite);
   public abstract void OnCollisionEnter(FiniteStateMachine finiteState);
   //This will be used to get the navmesh agent of the ai that has the finite state machine brain;
   public abstract void SetNavMeshAgent(NavMeshAgent parentAgent);
}