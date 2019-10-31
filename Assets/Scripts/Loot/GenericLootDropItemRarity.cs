using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PolyEQ.Utility;

namespace PolyEQ.Loot
{
    /// <summary>
    /// When we're inheriting, we have to insert GameObject as a type to GenericLootDropItem.
    /// </summary>
    [System.Serializable]
    public class GenericLootDropItemRarity : GenericLootDropItem<ItemRarity> { }
}