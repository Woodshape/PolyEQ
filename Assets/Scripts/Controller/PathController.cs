using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Controller
{
    public class PathController : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), 0.1f);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}