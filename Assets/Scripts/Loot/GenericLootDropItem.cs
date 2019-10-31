using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    public abstract class GenericLootDropItem<T>
    {
        //  Item it represents (i.e. GameObject, integer etc.)
        public T item;

        //  How many unit the item takes - more units = higher chance of being picked
        public float probabilityWeight;

        //  SHOULD NOT BE SET MANUALLY
        public float probabilityPercent;

        //  These values are assigned via LootDropTable script. 
        //  They represent from which number to which number if selected, the item will be picked.
        [HideInInspector]
        public float probabilityRangeFrom;
        [HideInInspector]
        public float probabilityRangeTo;
    }
}