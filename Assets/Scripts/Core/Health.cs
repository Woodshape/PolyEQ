using UnityEngine;

using PolyEQ.Controller;
using PolyEQ.Loot;
using PolyEQ.Stats;

namespace PolyEQ.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
                Die();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void Die()
        {
            if (isDead) { return; }

            GetComponent<ActionScheduler>().CancelCurrentAction();

            isDead = true;

            GetComponent<Animator>().SetTrigger("die");

            //TODO: make real loot behaviour.
            if (GetComponent<LootTarget>())
            {
                //  since it is possible that our loot target was spawned from code,
                //  it seems necessary to validate the loot table on death.
                GetComponent<LootTarget>().lootDropTable.ValidateTable();
                // GetComponent<LootTarget>().DropLootNearCorpse();
                GetComponent<LootTarget>().CreateLoot(GetComponent<LootTarget>().numItemsToDrop);
            }
        }
    }
}