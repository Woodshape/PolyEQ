using UnityEngine;

using PolyEQ.Loot;
using PolyEQ.Utility;
using PolyEQ.Core;

namespace PolyEQ.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "PolyEQ/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] GameObject pickupPrefab = null;
        [SerializeField] GameObject projectilePrefab = null;
        [SerializeField] ParticleSystem particlePrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] WeaponType weaponType;

        [SerializeField] float weaponRange = 1f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] float weaponSpeed = 1f;

        public GenericLootDropTableRarity itemRarityTable;

        ItemRarity itemRarity;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public GameObject Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (HasWeaponPrefab())
            {
                GameObject weaponGo;

                if (IsSpecialHandedWeapon())
                {
                    weaponGo = Instantiate(weaponPrefab, leftHand);
                    // weaponGo.transform.SetParent(leftHand, true);
                }
                else
                {
                    weaponGo = Instantiate(weaponPrefab, rightHand);
                }

                if (animatorOverride != null)
                {
                    animator.runtimeAnimatorController = animatorOverride;
                }

                if (HasGlowEffect())
                {
                    // Instantiate(particlePrefab, weaponGo.transform);

                    Light glow = weaponGo.AddComponent<Light>();
                    glow.color = GetGlowEffectColour();
                }

                return weaponGo;
            }

            return null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public GameObject SpawnProjectile(Transform spawnPos, Health target)
        {
            if (HasProjectilePrefab())
            {
                GameObject projectileGo;

                projectileGo = Instantiate(projectilePrefab, spawnPos.position, Quaternion.identity);
                projectileGo.GetComponent<Projectile>().SetupProjectile(target, GetWeaponDamage());

                return projectileGo;
            }

            return null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public GameObject Drop(Vector3 position)
        {
            if (HasPickupPrefab())
            {
                GameObject weaponGo = Instantiate(pickupPrefab, position, Quaternion.identity);

                if (HasGlowEffect())
                {
                    // Instantiate(particlePrefab, weaponGo.transform);

                    Light glow = weaponGo.AddComponent<Light>();
                    glow.color = GetGlowEffectColour();
                }

                return weaponGo;
            }

            return null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public Weapon CreateWeaponInstance(Weapon weapon)
        {
            //  If we equip a weapon, for now, we want to create a new instance of the weapon type
            //  and assign it a certain rarity.
            Weapon weaponToCreate = ScriptableObject.Instantiate(weapon);

            weaponToCreate.itemRarityTable.ValidateTable();
            weaponToCreate.CalculateWeaponRarity();

            Debug.Log("CREATED " + weaponToCreate + " of rarity " + weaponToCreate.GetWeaponRarity());

            return weaponToCreate;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnValidate()
        {
            itemRarityTable.ValidateTable();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CalculateWeaponRarity()
        {
            itemRarity = itemRarityTable.PickLootDropItem().item;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public WeaponType GetWeaponType()
        {
            return weaponType;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public ItemRarity GetWeaponRarity()
        {
            return itemRarity;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetWeaponDamage()
        {
            switch (GetWeaponRarity())
            {
                case ItemRarity.Poor:
                    return weaponDamage * 0.5f;
                case ItemRarity.Common:
                    return weaponDamage;
                case ItemRarity.Uncommon:
                    return weaponDamage * 1.5f;
                case ItemRarity.Rare:
                    return weaponDamage * 2.0f;
                case ItemRarity.Epic:
                    return weaponDamage * 5.0f;
                case ItemRarity.Legendary:
                    return weaponDamage * 10.0f;
                case ItemRarity.Unique: //  Unique items have pre-determined damage.
                    return weaponDamage;
                default:
                    return weaponDamage;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetWeaponDPS()
        {
            return GetWeaponDamage() / GetWeaponSpeed();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public float GetWeaponSpeed()
        {
            return weaponSpeed;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public Weapon CloneWeapon(Weapon weaponToClone)
        {
            //  This function creates an instance of the a certain weapon
            //  and assigns it the correct item rarity.
            Weapon cloneWeapon = ScriptableObject.Instantiate(weaponToClone);
            cloneWeapon.itemRarity = weaponToClone.GetWeaponRarity();

            return cloneWeapon;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool HasWeaponPrefab()
        {
            return weaponPrefab != null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool HasPickupPrefab()
        {
            return pickupPrefab != null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool HasProjectilePrefab()
        {
            return projectilePrefab != null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool HasGlowEffect()
        {
            switch (GetWeaponRarity())
            {
                case ItemRarity.Uncommon:
                case ItemRarity.Rare:
                case ItemRarity.Epic:
                case ItemRarity.Legendary:
                    return true;
                default:
                    return false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public Color GetGlowEffectColour()
        {
            switch (GetWeaponRarity())
            {
                case ItemRarity.Uncommon:
                    return Color.green;
                case ItemRarity.Rare:
                    return Color.blue;
                case ItemRarity.Epic:
                    return Color.magenta;
                case ItemRarity.Legendary:
                    return Color.red;
                default:
                    return Color.white;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool IsSpecialHandedWeapon()
        {
            //  Check to see if the weapon is of a special kind (i.e. requires a special hand to wield).
            switch (GetWeaponType())
            {
                case WeaponType.Mainhand:
                    return false;
                case WeaponType.Offhand:
                case WeaponType.TwoHanded:
                    return true;
                default:
                    return false;
            }
        }
    }
}