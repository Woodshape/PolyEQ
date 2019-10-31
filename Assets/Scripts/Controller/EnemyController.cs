using UnityEngine;
using System;

using PolyEQ.Movement;
using PolyEQ.Combat;
using PolyEQ.Core;


namespace PolyEQ.Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 5f;
        [SerializeField] PathController patrolPath = null;

        bool isPulled = false;

        public bool IsPulled
        {
            get { return isPulled; }
            set
            {
                isPulled = value;

                if (isPulled == true)
                {
                    timeSincePlayerPull = 0f;
                }
            }

        }

        GameObject target;

        Vector3 guardPosition;
        int currentWaypointIndex = 0;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSincePlayerPull = Mathf.Infinity;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");

            guardPosition = this.transform.position;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void Update()
        {
            if (GetComponent<Health>().IsDead()) { return; }

            if (IsInChaseDistance())
            {
                IsPulled = true;
            }
            else if (timeSincePlayerPull < 10.0f)    //  for now, we just hard code the "suspicion time"
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();

                if (IsPulled) { IsPulled = false; }
            }

            if (IsPulled)
            {
                AttackBehaviour();
            }

            UpdateTimers();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void PulledBy(GameObject target)
        {
            IsPulled = true;
            this.target = target;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateTimers()
        {
            timeSincePlayerPull += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private bool IsInChaseDistance()
        {
            return Vector3.Distance(this.transform.position, target.transform.position) <= chaseRadius;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void AttackBehaviour()
        {
            GetComponent<Fighter>().TryToAttack(target);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void GuardBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;

                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > 5f)
            {
                GetComponent<Mover>().TryToMove(nextPosition);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private Vector3 GetCurrentWaypoint()
        {
            Vector3 waypointPosition = patrolPath.GetWaypointPosition(currentWaypointIndex);

            return waypointPosition;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private bool AtWaypoint()
        {
            return Vector3.Distance(this.transform.position, GetCurrentWaypoint()) < 1.0f;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseRadius);
        }
    }
}