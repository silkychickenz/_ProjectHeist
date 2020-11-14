using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected readonly EnemyAI enemy;
    public State(EnemyAI Enemy)
    {
        enemy = Enemy;
    }

    public virtual IEnumerator Patrol()
    {
        yield break;
    }

    public virtual IEnumerator Detection()
    {
        yield break;
    }

    public virtual IEnumerator TakeCover()
    {
        yield break;
    }
}
