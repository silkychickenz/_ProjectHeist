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
        public bool breakTime = true;
        public State searchState;
        public Decision hearDecision;
        public Transform aimTarget;
        public float tooFarFromTarget = 50;
        AudioSource audioSource;

        private void Awake()
        {
            if (aimTarget == null)
                aimTarget = GameObject.FindObjectOfType<playerController>().gameObject.transform;
        }

        private void Start()
        {  
            audioSource = GetComponent<AudioSource>();
            for(int i =0; i<squad.Count;i++)
            {
                squad[i].m_squad=this;
                squad[i].aimTarget = aimTarget;
            }
            BreakTimeStart();
        }

        private void LateUpdate()
        {
            if (Vector3.Distance(aimTarget.position, transform.position) <= tooFarFromTarget)
            {
                if (breakTime == true)
                {
                    breakTime = false;
                    BreakTimeOver();
                }
            }
            else
            {
                if(breakTime == false)
                {
                    breakTime = true;
                    BreakTimeStart();
                }
                
            }

        }
        public void BreakTimeStart() 
        {
            Debug.Log(gameObject.transform.parent.name + " is taking a break");
            for (int i = 0; i < squad.Count; i++)
            {
                
                squad[i].enabled = false;
                squad[i].spotLight.enabled = false;
            }
        }
           

        public void BreakTimeOver()
        {
            Debug.Log(gameObject.transform.parent.name + " is off break");
            for (int i = 0; i < squad.Count; i++)
            {                
                squad[i].enabled = true;
                squad[i].spotLight.enabled = true;
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