using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
        public int damage = 100;
        public float attackRange = 1.0f;
        public LayerMask Player;

        void Update()
        {
            // Check if the player pressed the attack button
            if (Input.GetKey(KeyCode.Space))
            {
                // Check if there is an enemy within the attack range
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, Player);
                if (hitEnemies.Length > 0)
                {
                    // Damage the first enemy hit
                    hitEnemies[0].GetComponent<EnemyHealth>().TakeDamage(damage);
                }
            }
        }
    

}
