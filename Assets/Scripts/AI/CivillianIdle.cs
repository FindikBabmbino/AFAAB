using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivillianIdle : StatePattern
{
    private NavMeshAgent agent;
    private GameObject player;
     //Since this needs an agent we gotta have the bool as well.
    private bool needsAgent;
    //This will contain every waypoint in the map
    private List<GameObject> wayPoints=new List<GameObject>();

    private GameObject currentWaypoint;

     public override void EnterState(FiniteStateMachine finiteState)
    {
        needsAgent=true;
        //Get all the waypoints and store them here
        PopulateWaypointList();
    }

    public override void UpdateState(FiniteStateMachine finiteState)
    {  
        DecideWayPointToGoTo();
        
        //If everything is removed from the list populated again so that the ai can find somewhere to go again.
        if(wayPoints.Count<=0)
        {
            PopulateWaypointList();
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

    //Thıs wıll handle where the ai will go, it will get the closest waypoint and see if the ai has allready crossed it.
    private void DecideWayPointToGoTo()
    {
        float minDistance=Mathf.Infinity;

        foreach(var waypoint in wayPoints)
        {
            //This gets the distance of the waypoint
            Vector3 distanceToWayPoint=waypoint.transform.position-agent.gameObject.transform.position;
            //Get the squared magnitude it is not linear so it is more optimised
            float distanceSqrd=distanceToWayPoint.sqrMagnitude;
            if(distanceSqrd<=minDistance)
            {
                currentWaypoint=waypoint;
                minDistance=distanceSqrd;
                agent.SetDestination(currentWaypoint.transform.position);
            }
        }
        //If the remaning distance is less then 1 then the ai has allready reached the goal
        if(agent.remainingDistance<=1)
        {
            //Remove the current waypoint from the list so that the ai does not go there anymore.
            wayPoints.Remove(currentWaypoint);
            //Also set this to null to be sure
            currentWaypoint=null;
        }
    }

    //This function will be called to populate the waypoint list
    private void PopulateWaypointList()
    {
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
    }
}

