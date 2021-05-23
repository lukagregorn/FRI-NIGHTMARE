using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quizko : Enemy
{

    public GameObject projectile;

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (path != null) {
            if (!ReachedPathEnd() && !IsInAttackRadius()) {
                Vector2 direction = MoveTowardsTarget();
                AnimateSpriteDirection(direction);
            }
        }

    }


    // MOVEMENT
    private void AnimateSpriteDirection(Vector2 direction) {
        if (direction.x > 0) {
            animator.SetFloat("moveX", 1f);
        } else {
            animator.SetFloat("moveX", -1f);
        }
    }


    // COMBAT
    public override IEnumerator AttackCoroutine() {
        while (!IsDead()) {
            yield return new WaitForSeconds(4.0f);
            if (IsInAttackRadius()) {
                
                // get values
                Vector3 tempVector = target.transform.position - transform.position;
                float knockTime = GetKnockTime();
                float knockThrust = GetKnockThrust();
                int damage = GetDamage();

                // instantiate projectile
                GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                
                // fire towards player
                current.GetComponent<Projectile>().Launch(tempVector, damage, knockTime, knockThrust);

            }
        }
    }
    

}
