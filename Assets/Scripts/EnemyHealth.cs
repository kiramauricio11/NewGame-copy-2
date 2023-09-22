using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float maxHealth = 100f; // the maximum health of the enemy
    public float currentHealth; // the current health of the enemy
    public bool isDead = false; // a flag to indicate if the enemy is dead or not
    public Transform EnemySoldier;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // set the current health to the maximum health at the start
    }

    // Update is called once per frame
    void Update()
    {
        // check if the enemy is dead
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true; // set the isDead flag to true
            OnDeath(); // call the OnDeath function
        }
    }

    // a function to apply damage to the enemy
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // reduce the current health by the amount of damage taken
    }

    // a function to handle the enemy's death
    public void OnDeath()
    {
        // add any logic you want to happen when the enemy dies here
        // for example, you might want to play a death animation or destroy the enemy game object
        Destroy(EnemySoldier); // destroy the enemy game object
    }
}
