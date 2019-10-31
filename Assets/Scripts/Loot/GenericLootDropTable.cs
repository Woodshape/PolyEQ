using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    /// <summary>
    /// Class serves for assigning and picking loot drop items.
    /// </summary>
    public abstract class GenericLootDropTable<T, U> where T : GenericLootDropItem<U>
    {
        [SerializeField]
        public List<T> lootDropItems;

        //  Sum of all weights of items.
        float probabilityTotalWeight;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the percentage and asigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary> 
        public void ValidateTable()
        {
            if (lootDropItems != null && lootDropItems.Count > 0)
            {
                float currentProbabilityWeightMaximum = 0f;

                foreach (var lootDropItem in lootDropItems)
                {
                    if (lootDropItem.probabilityWeight < 0f)
                    {
                        Debug.Log("ERROR: you can't have negative weights. Resetting weight to 0.");
                        lootDropItem.probabilityWeight = 0f;
                    }
                    else
                    {
                        //  Arrange loot drop items in a row based on their weight.
                        lootDropItem.probabilityRangeFrom = currentProbabilityWeightMaximum;
                        currentProbabilityWeightMaximum += lootDropItem.probabilityWeight;
                        lootDropItem.probabilityRangeTo = currentProbabilityWeightMaximum;
                    }
                }

                probabilityTotalWeight = currentProbabilityWeightMaximum;

                foreach (var lootDropItem in lootDropItems)
                {
                    //  Calculate percentage of item drop select rate.
                    lootDropItem.probabilityPercent = ((lootDropItem.probabilityWeight) / probabilityTotalWeight) * 100;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Picks and returns the loot drop item based on it's probability.
        /// </summary>
        public T PickLootDropItem()
        {
            float pickNumber = Random.Range(0, probabilityTotalWeight);

            foreach (var lootDropItem in lootDropItems)
            {
                //  If the picked number lies within the item's range, return item.
                if (pickNumber > lootDropItem.probabilityRangeFrom && pickNumber < lootDropItem.probabilityRangeTo)
                {
                    return lootDropItem;
                }
            }

            Debug.LogError("Item couldn't be picked! Be sure that all of your active loot drop tables have assigned at least one item!");
            return lootDropItems[0];
        }
    }
}