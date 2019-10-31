using UnityEngine;

using PolyEQ.Interface;

namespace PolyEQ.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public void StartAction(IAction action)
        {
            if (currentAction == action) { return; }

            if (currentAction != null)
            {
                Debug.Log(currentAction);
                currentAction.CancelAction();
            }

            currentAction = action;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CancelCurrentAction()
        {
            //  
            StartAction(null);
        }
    }
}