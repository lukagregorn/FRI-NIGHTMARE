using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (path != null) {
            if (!ReachedPathEnd()) {
                Vector2 direction = MoveTowardsTarget();
            }
        }

    }

    // COMBAT
    private void OnTriggerEnter2D(Collider2D other) {
        // check if player
        if (other.gameObject.CompareTag("Player") && other.isTrigger) {

            animator.SetTrigger("attack");

            // get humanoid object
            Humanoid playerHuman = other.GetComponent<Humanoid>();
            
            // get values
            float knockTime = GetKnockTime();
            float knockThrust = GetKnockThrust();
            int damage = GetDamage();

            // calculate knock vector
            Vector2 knockVector = other.GetComponent<Rigidbody2D>().transform.position - transform.position;
            knockVector = knockVector.normalized * knockThrust;

            // inflict knockback and damage
            playerHuman.TakeDamage(damage);
            playerHuman.Knockback(knockVector, knockTime);
        }
    }
    

}
