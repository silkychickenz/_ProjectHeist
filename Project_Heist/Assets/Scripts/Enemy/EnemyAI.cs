using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : StateMachine
{
   
    private void Start()
    {
        SetState(new Patrolling(this));
    }

}
