using System.Collections;
using System.Collections.Generic;
using PolyEQ.Combat;
using UnityEngine;

namespace PolyEQ.Loot
{
    public class TreasureChest : MonoBehaviour
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
        void Start()
        {
            CreateLoot(numItemsToDrop);
            // DropLootNearChest(numItemsToDrop);
            // DropCoins(numOfCoins);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnValidate()
        {
            //  Validate Loot Table
            itemDropTable.ValidateTable();
            lootDropTable.ValidateTable();
            coinDropTable.ValidateTable();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        void CreateLoot(int numberOfItems)
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

        //////////////////////////////////////////////////////////////////////////////////////////
        void DropLootNearChest(int numItemsToDrop)
        {
            for (int i = 0; i < numItemsToDrop; i++)
            {
                GenericLootDropItemGameObject selectedItem = lootDropTable.PickLootDropItem();

                GameObject selectedItemGameObject = Instantiate(selectedItem.item);
                selectedItemGameObject.transform.position = new Vector3(
                    this.transform.position.x + Random.Range(-xOff, xOff),
                    this.transform.position.y + 0.5f,
                    this.transform.position.z + Random.Range(-zOff, zOff));
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        void DropCoins(int numberOfCoins)
        {
            for (int i = 0; i < numberOfCoins; i++)
            {
                GenericLootDropItemInteger selectedCoins = coinDropTable.PickLootDropItem();
            }
        }
    }
}