using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Loot
{
    public class LootTarget : MonoBehaviour
    {

        public GenericLootDropTableGameObject lootDropTable;
        public GenericLootDropTableInteger coinDropTable;

        public int numItemsToDrop;

        public int numOfCoins;

        public float xOff, zOff;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnValidate()
        {
            //  Validate Loot Table
            lootDropTable.ValidateTable();
            coinDropTable.ValidateTable();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void DropLootNearCorpse()
        {
            for (int i = 0; i < numItemsToDrop; i++)
            {
                GenericLootDropItemGameObject selectedItem = lootDropTable.PickLootDropItem();

                //TODO: implement better looting system!
                //  Fow now, we check if we have an item (item gameobject) to account for loot percentages
                //  (i.e. selectedItem[0] == no gameobject) to make it possible to now drop loot on death.
                if (selectedItem.item != null)
                {
                    GameObject selectedItemGameObject = Instantiate(selectedItem.item);
                    selectedItemGameObject.transform.position = new Vector3(
                        this.transform.position.x + Random.Range(-xOff, xOff),
                        this.transform.position.y,
                        this.transform.position.z + Random.Range(-zOff, zOff));
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void DropCoins()
        {
            for (int i = 0; i < numOfCoins; i++)
            {
                GenericLootDropItemInteger selectedCoins = coinDropTable.PickLootDropItem();

                Debug.Log(selectedCoins.item);
            }
        }
    }
}