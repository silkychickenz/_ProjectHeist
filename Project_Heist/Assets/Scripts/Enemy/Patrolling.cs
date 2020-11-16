using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : State
{
    WayPointSystem navigation;
    public Patrolling(EnemyAI enemy) : base(enemy)
    {
       
    }

    public override IEnumerator Patrol()
    {
        Debug.Log("pee");
        yield return new WaitForSeconds(1f);
    }

   
}
