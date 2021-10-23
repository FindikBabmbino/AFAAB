using UnityEngine;
using UnityEngine.AI;

public class GenericIdle : StatePattern
{
    //This state will be used for the generic walk around town state.
    //When the enemy detects Sarp in the overworld they will switch to chase state
    private NavMeshAgent agent;
    //We will get a reference to player to calculate the see angle
    private GameObject player;
    private float noticeRange=100.0f;
    private float noticeAngle=40.0f;
    private float engageCombatDistance=5.0f;
    private float distanceToRandomPoint=3.0f;
    private float sphereRadious=10.0f;
    //If this is false it wont take an agent
    private bool needsAgent;
    public override void EnterState(FiniteStateMachine finiteState)
    {
        //TODO also get player here-DOne
        player=GameObject.FindGameObjectWithTag("Player");
        needsAgent=true;
    }

    public override void UpdateState(FiniteStateMachine finiteState)
    {  
        //We get the direction rather then the distance so we can calculate the angle of the player.
        Vector3 direction=player.transform.position-agent.transform.position;
        float angle=Vector3.Angle(direction,player.transform.forward);
        if(direction.magnitude<=noticeRange&&angle<=noticeAngle)
        {
            //Debug.Log("I see you");
            //do a vector.zero so we can reset the destination before setting it again.
            agent.SetDestination(Vector3.zero);
            agent.SetDestination(player.transform.position);
            //Set the notice angle to 180 so the ai can actually chase the player.
            //This is actually a logic error thanks to this player will never be able to escape the enemy will come back to this 
            noticeAngle=180.0f;
        }
        else if(agent.remainingDistance<=agent.stoppingDistance)
        {
            //Debug.Log("Back to wandering around");
            //do a vector.zero so we can reset the destination before setting it again.
            agent.SetDestination(Vector3.zero);
            agent.SetDestination(CalculateWanderLocation());
            //Set it back the notice angle to 40
            noticeAngle=40.0f;
        }
        //If the direction magnitude is in the combat distance we change the state and call setisinbattle function from the singleton
        if(direction.magnitude<=engageCombatDistance)
        {
            CombatEventSystemManager.instance.SetIsInBattle(true);
            //This will be enabled when we have the combat state and the setisinbattle will probably be moved there as well.
            finiteState.SwitchState(finiteState.combatState);
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
            Debug.Log("Does not need an agent");
            return;
        }
        agent=parentAgent;
    }
    //No idea how this works lol 'learned' it from some guys video who did not even talk.
    private Vector3 CalculateWanderLocation()
    {
        Vector3 finalPostion=Vector3.zero;
        Vector3 randomPos=Random.insideUnitSphere*sphereRadious;
        randomPos+= agent.gameObject.transform.position;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPos,out hit,sphereRadious,1))
        {
            finalPostion=hit.position;
        }
        return finalPostion;
    }
}