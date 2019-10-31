using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PolyEQ.Core;

namespace PolyEQ.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Health target;
        [SerializeField] float projectileSpeed = 1f;

        float damage = 0f;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        void Update()
        {
            if (target == null) { return; }

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        Vector3 GetAimLocation()
        {
            //  We want to aim at the target's center of mass, which is partly defined by its collider size.
            Collider targetCollider = target.GetComponent<Collider>();

            if (targetCollider == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCollider.bounds.size.z / 2;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void SetupProjectile(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) { return; }

            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(damage);
            Destroy(gameObject);
        }
    }
}