using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyEQ.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponToPickup;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().PickupWeapon(weaponToPickup);
                Destroy(this.gameObject);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void SetWeaponPickup(Weapon weapon)
        {
            //  We have to make sure to "synch" the weapon we want to pick up with the weapon data.
            weaponToPickup = weaponToPickup.CloneWeapon(weapon);

            Debug.Log("Setting Pickup to: " + weaponToPickup + " with " + weaponToPickup.GetWeaponRarity());
        }
    }
}