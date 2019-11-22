using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyEQ.Combat;

namespace PolyEQ.Loot
{
    public class LootTarget : MonoBehaviour
    {

        public GenericLootDropTableGameObject lootDropTable;
        public GenericLootDropTableScriptableObject itemDropTable;
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
            itemDropTable.ValidateTable();
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

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CreateLoot(int numberOfItems)
        {
            itemDropTable.ValidateTable();

            for (int i = 0; i < numberOfItems; i++)
            {
                GenericLootDropItemScriptableObject selectedItem = itemDropTable.PickLootDropItem();

                //  For now we just want to get a weapon from the loot list, create an instance of the weapon
                //  and then drop that instance clone with the correct metadata (i.e. rarity) on the ground (to pickup).
                //  TODO: Handle other cases of scriptable objects!
                Weapon selectedItemWeapon = ScriptableObject.Instantiate((Weapon)selectedItem.item);
                Weapon weaponInstance = selectedItemWeapon.CreateWeaponInstance(selectedItemWeapon);
                Weapon weaponToDrop = weaponInstance.CloneWeapon(weaponInstance);

                Debug.Log("Creating Loot Chest Item: " + weaponToDrop + " / " + weaponToDrop.GetWeaponRarity());

                DropItem(weaponToDrop);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        void DropItem(Weapon item)
        {
            GameObject prefab = item.Drop(new Vector3(
                    this.transform.position.x + Random.Range(-xOff, xOff),
                    this.transform.position.y + 0.5f,
                    this.transform.position.z + Random.Range(-zOff, zOff)));

            //  Make sure to set the correct weapon to drop on the ground.
            prefab.GetComponent<WeaponPickup>().SetWeaponPickup(item);
        }
    }
}