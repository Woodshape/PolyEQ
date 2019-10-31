using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    /// <summary>
    /// When we're inheriting, we have to insert ScriptableObject as a type to GenericLootDropItem.
    /// </summary>
    [System.Serializable]
    public class GenericLootDropItemScriptableObject : GenericLootDropItem<ScriptableObject> { }
}
