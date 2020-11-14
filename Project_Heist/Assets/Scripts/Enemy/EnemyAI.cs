using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : StateMachine
{
    public List<GameObject> targets;
    public int wayPointIndex;

    public UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Start()
    {
       
        SetState(new StartState());
    }

}
