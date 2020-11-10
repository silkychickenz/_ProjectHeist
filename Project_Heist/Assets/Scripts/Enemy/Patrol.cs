using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public List<GameObject> targets;
    public int wayPointIndex;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targets[wayPointIndex].transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targets[wayPointIndex])
        {
            wayPointIndex = (wayPointIndex + 1) % targets.Count; //Find next waypoint in list after reaching one
        }
    }
}
