using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PolyEQ.Movement;
using PolyEQ.Combat;
using PolyEQ.Core;

namespace PolyEQ.Controller
{
    public class PlayerController : MonoBehaviour
    {

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        void Update()
        {
            if (GetComponent<Health>().IsDead()) { return; }
            //  For now, we basically have two interaction options: attack or move
            //  The attack action should be our primary concern - if we attack, we 
            //  don't want to call the standard move action.
            if (InteractWithCombat()) { return; }

            if (InteractWithMovement()) { return; }

            // Debug.Log("NOTHING TO DO HERE - WE CANNOT MOVE OR ATTACK");
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private bool InteractWithCombat()
        {
            //  For our combat we want to check for the Combat Target Component to 
            //  make durr our target is an entity that can be targetet.
            RaycastHit[] hits = Physics.RaycastAll(GetCursorRay());

            foreach (var hit in hits)
            {
                CombatTarget combatTarget = hit.collider.GetComponent<CombatTarget>();
                if (combatTarget == null) { continue; }

                if (!GetComponent<Fighter>().CanAttack(combatTarget.gameObject)) { continue; }

                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<Fighter>().TryToAttack(combatTarget.gameObject);
                }
                //  We want to return true here so we get the bool even if we just hover
                //  over target with the cursor (TODO: special mouse behaviour).
                return true;
            }

            return false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private static Ray GetCursorRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetCursorRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    this.GetComponent<Mover>().TryToMove(hit.point);
                }
                //  We want to return true here so we get the bool even if we just hover
                //  over target with the cursor (TODO: special mouse behaviour).
                return true;
            }

            return false;
        }
    }
}