using System;
using System.Collections.Generic;
using UnityEngine;

using PolyEQ.Utility;

namespace PolyEQ.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "PolyEQ/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetHealthForLevel(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass cc in characterClasses)
            {
                if (cc.characterClass == characterClass)
                {
                    return cc.health[level - 1];
                }
            }

            Debug.LogWarning("NO HEALTH PER LEVEL DEFINED FOR LEVEL " + level + " ON " + characterClass);
            return 0f;
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }
    }
}
