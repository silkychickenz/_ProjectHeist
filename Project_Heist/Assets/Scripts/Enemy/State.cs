using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual IEnumerator Start()
    {
        yield break;
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
