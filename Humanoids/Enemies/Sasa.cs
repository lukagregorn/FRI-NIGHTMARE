using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sasa : Enemy
{

    public Sprite close;
    public Sprite far;
    public SpriteRenderer sr;

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (path != null) {
            if (!ReachedPathEnd()) {
                Vector2 direction = MoveTowardsTarget();
            }

            if (IsInAttackRadius()) {
                sr.sprite = close;

            } else {
                sr.sprite = far;

            }
        }

    }

    // COMBAT
    private void OnTriggerEnter2D(Collider2D other) {
        // check if player
        if (other.gameObject.CompareTag("Player") && other.isTrigger) {

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
