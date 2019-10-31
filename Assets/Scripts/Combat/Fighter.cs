using UnityEngine;

using PolyEQ.Core;
using PolyEQ.Interface;
using PolyEQ.Movement;
using PolyEQ.Controller;

namespace PolyEQ.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Weapon startingWeapon = null;
        Weapon equippedWeapon;
        GameObject equippedWeaponGo;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void Start()
        {
            EquipWeapon(startingWeapon);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void Update()
        {
            if (target == null || target.IsDead()) { return; }

            timeSinceLastAttack += Time.deltaTime;

            //  Whenever we have a target (i.e. we are in combat), 
            //  we want to start moving toward its position.
            if (!IsTargetInRange())
            {
                this.GetComponent<Mover>().MoveTowards(target.transform.position);
            }
            else
            {
                //  Ok, we reached the enemy position, so we can start attack behaviour.
                AttackBehaviour();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) { return; }

            //  Make sure to destroy the old weapon we have equipped.
            //  TODO: implement real equip and inventory system.
            if (equippedWeapon != weapon)
            {
                equippedWeapon = null;
                Destroy(equippedWeaponGo);
            }

            //  If we equip a weapon, for now, we want to create a new instance of the weapon type
            //  and assign it a certain rarity.
            equippedWeapon = weapon.CreateWeaponInstance(weapon);

            // Next, we want to spawn the weapon data in a game object and parent it to the player's hand(s).
            Animator animator = GetComponent<Animator>();
            equippedWeaponGo = equippedWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void PickupWeapon(Weapon weapon)
        {
            if (weapon == null) { return; }

            //  Make sure to destroy the old weapon we have equipped.
            //  TODO: implement real equip and inventory system.
            if (equippedWeapon != weapon)
            {
                equippedWeapon = null;
                Destroy(equippedWeaponGo);
            }

            equippedWeapon = weapon;

            Animator animator = GetComponent<Animator>();
            equippedWeaponGo = equippedWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnValidate()
        {
            if (equippedWeapon == null) { return; }

            //  We want to update our weapon prefab if we assign a new Weapon SO in the inspector.
            if (equippedWeapon.HasWeaponPrefab())
            {
                EquipWeapon(equippedWeapon);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void AttackBehaviour()
        {
            //  For now: make sure we are facing the enemy (TODO: fix buggy snappy turns)
            //  and stop movement (+ handle animation)
            this.transform.LookAt(target.transform);
            this.GetComponent<Mover>().CancelAction();

            if (timeSinceLastAttack > equippedWeapon.GetWeaponSpeed())
            {
                //  The attack animation will trigger our Hit Animation Event (below).
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //  Animation Event
        void Hit()
        {
            if (target == null) { return; }

            target.TakeDamage(equippedWeapon.GetWeaponDamage());

            EnemyAggro();

            Debug.Log(equippedWeapon.GetWeaponDamage());
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //  Animation Event
        void Shoot()
        {
            if (target == null) { return; }

            equippedWeapon.SpawnProjectile(equippedWeaponGo.transform, target);

            EnemyAggro();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void EnemyAggro()
        {
            if (target.GetComponent<EnemyController>())
            {
                target.GetComponent<EnemyController>().PulledBy(this.gameObject);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private bool IsTargetInRange()
        {
            return Vector3.Distance(this.transform.position, target.transform.position) < equippedWeapon.GetWeaponRange();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void TryToAttack(GameObject target)
        {
            //  This function will get called when we click on an enemy for example.
            //  We want to tell our action manager that we are now in combat mode
            //  and set our target.
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.GetComponent<Health>();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public bool CanAttack(GameObject target)
        {
            if (this.GetComponent<Health>().IsDead()) { return false; }

            return (target != null && !target.GetComponent<Health>().IsDead());
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CancelAction()
        {
            UntriggerAttack();
            this.target = null;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void TriggerAttack()
        {
            //  Sets and resets our attack animations.
            this.GetComponent<Animator>().ResetTrigger("stop_attack");
            this.GetComponent<Animator>().SetTrigger("attack_1");
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void UntriggerAttack()
        {
            //  Sets and resets our attack animations.
            this.GetComponent<Animator>().ResetTrigger("attack_1");
            this.GetComponent<Animator>().SetTrigger("stop_attack");
        }
    }
}