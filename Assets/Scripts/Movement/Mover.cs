using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using PolyEQ.Core;
using PolyEQ.Interface;

namespace PolyEQ.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private Transform target;

        private NavMeshAgent myAgent;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        void Start()
        {
            myAgent = GetComponent<NavMeshAgent>();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        void Update()
        {
            myAgent.enabled = !GetComponent<Health>().IsDead();

            UpdateAnimator();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void TryToMove(Vector3 dest)
        {
            //  This function will cancel any other actions we were performing
            //  and then move towards the destination (i.e. movement cancels attack).
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTowards(dest);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void MoveTowards(Vector3 dest)
        {
            myAgent.SetDestination(dest);
            myAgent.isStopped = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateAnimator()
        {
            if (myAgent.enabled == false) { return; }

            if (myAgent.isStopped == true)
            {
                GetComponent<Animator>().SetBool("run", false);
            }
            else
            {
                GetComponent<Animator>().SetBool("run", true);
            }

            if (Vector3.Distance(myAgent.destination, this.transform.position) <= 0.1f)
            {
                CancelAction();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CancelAction()
        {
            myAgent.isStopped = true;
        }
    }
}