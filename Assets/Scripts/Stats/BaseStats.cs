using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PolyEQ.Utility;

namespace PolyEQ.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetHealth()
        {
            return progression.GetHealthForLevel(characterClass, startingLevel);
        }
    }
}