using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    /// <summary>
    /// When we're inheriting, we have to insert Int as a type to GenericLootDropItem.
    /// </summary>
    [System.Serializable]
    public class GenericLootDropItemInteger : GenericLootDropItem<int> { }
}