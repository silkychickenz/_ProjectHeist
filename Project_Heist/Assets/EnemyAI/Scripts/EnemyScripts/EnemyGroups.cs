using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public class EnemyGroups : MonoBehaviour
    {
        public List<StateController> squad;
        public bool alarmOn;
        public bool breakTime = true;
        public State searchState;
        public State patrolState;
        public Decision hearDecision;
        public Transform aimTarget;
        public float tooFarFromTarget = 50;
        public GameObject gameController;
        AudioSource audioSource;
        CoverLookup coverLookup;

        private void Awake()
        {   
            coverLookup = GameObject.FindObjectOfType<CoverLookup>().GetComponent<CoverLookup>();
            coverLookup.Setup(squad[0].generalStats.coverMask);
            audioSource = GetComponent<AudioSource>();
            if (aimTarget == null)
                aimTarget = GameObject.FindObjectOfType<playerController>().gameObject.transform;
            foreach (StateController enemy in squad)
            {
                enemy.m_squad = this;
                enemy.aimTarget = aimTarget;
                enemy.coverLookup = coverLookup;
            }
        }

        private void Start()
        {
            BreakTimeStart();
        }
        void LateUpdate()
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
                if (breakTime == false)
                {
                    breakTime = true;
                    BreakTimeStart();
                }
            }
        }

        public void BreakTimeStart() 
        {
            Debug.Log(gameObject.transform.parent.name + " is taking a break");
            foreach (StateController enemy in squad)
            {                
                enemy.enabled = false;
                enemy.spotLight.enabled = false;
            }
        }
           

        public void BreakTimeOver()
        {
            Debug.Log(gameObject.transform.parent.name + " is off break");
            foreach (StateController enemy in squad)
            {                
                enemy.enabled = true;
                enemy.spotLight.enabled = true;
            }
        }

        public void SoundAlarm(Vector3 lastKnownLocation)
        {
            alarmOn = true;
            Debug.Log("AlarmOn");
            audioSource.Play();
            foreach (StateController enemy in squad)
            {
                enemy.TransitionToState(searchState, hearDecision);
                enemy.personalTarget = lastKnownLocation;
            }
        }

        public void UpdateLastKnownLocation(Vector3 lastKnownLocation)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(lastKnownLocation, out hit, 1.0f, NavMesh.AllAreas))
            {
                lastKnownLocation = hit.position;                
            }
            else
            {
                return;
            }

            foreach (StateController enemy in squad)
            {
                enemy.personalTarget = lastKnownLocation;                
            }
        }

        public void TurnOffAlarm()
        {
            alarmOn = false;
            Debug.Log("AlarmOff");
            foreach (StateController enemy in squad)
            {
                enemy.TransitionToState(patrolState, hearDecision);
            }
        }

        internal void RemoveSelf(StateController stateController)
        {
            squad.Remove(stateController);
        }
    }
}