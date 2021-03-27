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
        public State searchState;
        public Decision hearDecision;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            for(int i =0; i<squad.Count;i++)
            {
                squad[i].m_squad=this;
            }
        }

        public void SoundAlarm(Vector3 lastKnownLocation)
        {
            alarmOn = true;
            Debug.Log("AlarmOn");
            audioSource.Play();
            for(int i = 0; i <= squad.Count-1; i++)
            {
                squad[i].TransitionToState(searchState, hearDecision);
                squad[i].personalTarget = lastKnownLocation;
            }
        }

        internal void RemoveSelf(StateController stateController)
        {
            squad.Remove(stateController);
        }
    }
}