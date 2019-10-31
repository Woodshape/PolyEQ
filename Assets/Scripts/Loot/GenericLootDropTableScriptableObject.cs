using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    /// <summary>
    /// When inheriting, first we have to insert GenericLootDropItemScriptableObject instead of T and a ScriptableObject instead of U
    /// </summary>
    [System.Serializable]
    public class GenericLootDropTableScriptableObject : GenericLootDropTable<GenericLootDropItemScriptableObject, ScriptableObject> { }
}
