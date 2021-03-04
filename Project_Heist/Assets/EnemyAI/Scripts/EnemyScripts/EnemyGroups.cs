using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    public class EnemyGroups : MonoBehaviour
    {
        public List<StateController> squad;
        public bool alarmOn;
        public State FindCoverState;
        public Decision FeelAlertDecision;


        public void SoundAlarm(Vector3 lastKnownLocation)
        {
            alarmOn = true;
            Debug.Log("AlarmOn");
            for(int i = 0; i <= squad.Count; i++)
            {
                squad[i].TransitionToState(FindCoverState, FeelAlertDecision);
            }
        }

        internal void RemoveSelf(StateController stateController)
        {
            squad.Remove(stateController);
        }
    }
}