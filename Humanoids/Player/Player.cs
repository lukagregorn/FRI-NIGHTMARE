using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Humanoid
{
    public Signal playerHealthSignal;
    public Signal playerDiedSignal;
    public Signal playerCoinsSignal;
    public GameObject contextClue;
    public Vector3 initialPosition;
    private Vector3 moveChange;

    // Start is called before the first frame update
    private void Start() {
        
        // initial health
        if (maxHealth % 2 != 0)
            Debug.LogWarning("Player max health is not even. Hearts may not show correctly");
        health = maxHealth;
        playerHealthSignal.Fire();

        // initial direction
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        transform.position = initialPosition;
    }

    // Update is called once per frame
    private void Update() {
        HumanoidState state = GetState();

        if (Input.GetButtonDown("Attack") && state != HumanoidState.attack
                && state != HumanoidState.stagger) {
            StartCoroutine(AttackCoroutine());
        }

    }


    // FixedUpdate is called when physics update
    private void FixedUpdate() {
        // reset change
        moveChange = Vector3.zero;

        // get new change
        moveChange.x = Input.GetAxisRaw("Horizontal");
        moveChange.y = Input.GetAxisRaw("Vertical");
        
        HumanoidState state = GetState();

        if (state == HumanoidState.walk || state == HumanoidState.idle) {
            // move character
            UpdateAnimationAndMove();
        }
    }
 

    // MOVEMENT
    private void UpdateAnimationAndMove() {
        if (moveChange != Vector3.zero) {
            MoveCharacter();

            // animator blend tree changes
            animator.SetFloat("moveX", moveChange.x);
            animator.SetFloat("moveY", moveChange.y);
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }
    }

    private void MoveCharacter()
    {
        moveChange.Normalize();
        myRigidbody.MovePosition(
            transform.position + moveChange * speed * Time.deltaTime
        );
    }


    // COMBAT
    public void OnAttack() {
        HumanoidState state = GetState();

        if (state != HumanoidState.attack && state != HumanoidState.stagger) {
            StartCoroutine(AttackCoroutine());
        }
    }


    private IEnumerator AttackCoroutine() {
        ChangeState(HumanoidState.attack);
        animator.SetBool("attacking", true);
        yield return null;
        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.33f);

        ChangeState(HumanoidState.walk);
    }

    
    public override void TakeDamage(int damage) {
        
        health -= damage;
        playerHealthSignal.Fire();

        if (health <= 0) {
            OnDeath();
        }

    }


    public override void OnDeath() {
        ChangeState(HumanoidState.dead);
        
        health = 0;
        playerHealthSignal.Fire();

        // fire death signal
        playerDiedSignal.Fire();
        gameObject.SetActive(false);
    }


    //MISC
    public void SetContextClue(Sprite clueSprite, bool visible) {
        if (enabled && clueSprite) {
            contextClue.GetComponent<SpriteRenderer>().sprite = clueSprite;
        }
        contextClue.SetActive(visible);
    }

}
