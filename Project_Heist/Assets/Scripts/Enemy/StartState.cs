using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    public EnemyAI ai;

    public override IEnumerator Start()
    {
        ai.agent.SetDestination(ai.targets[ai.wayPointIndex].transform.position); //Set distance towards way points
        yield return new WaitForSeconds(1f);
    }
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ai.targets[ai.wayPointIndex])
        {
            ai.wayPointIndex = (ai.wayPointIndex + 1) % ai.targets.Count; //Find next waypoint in list after reaching one
        }
    }
}
