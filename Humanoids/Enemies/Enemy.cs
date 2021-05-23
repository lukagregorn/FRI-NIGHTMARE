using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;


public class Enemy : Humanoid
{
    public Transform home;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public UnityEvent DiedEvent;
    
    protected Seeker seeker;
    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;
    private float nextWaypointDistance = 1.5f;

    private void Start() {
        health = maxHealth;
        GameObject p = GameObject.FindWithTag("Player");
        if (!p) {
            Debug.LogWarning("No player transform");
            return;
        }

        // start attack coroutine
        StartCoroutine(AttackCoroutine());

        // ai
        target = p.transform;
        seeker = GetComponent<Seeker>();

        // generate path
        InvokeRepeating("UpdatePath", 0f, .5f);
    }


    // MOVEMENT
    protected void UpdatePath() {
        if (seeker.IsDone())
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
    }

    protected bool ReachedPathEnd() {
        reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;
        return reachedEndOfPath;
    }

    protected void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected Vector2 MoveTowardsTarget() {
        HumanoidState state = GetState();
        if (state == HumanoidState.idle || state == HumanoidState.walk) {
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            myRigidbody.AddForce(force);

            float distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) {
                currentWaypoint ++;
            }

            ChangeState(HumanoidState.walk);

            // returns direction
            return direction;
        }

        return Vector3.zero;
    }

    protected bool IsInChaseRadius() {
        return (Vector3.Distance(target.position, transform.position) <= chaseRadius);
    }


    protected bool IsInAttackRadius() {
        return (Vector3.Distance(target.position, transform.position) <= attackRadius);
    }


    // COMBAT
    public override void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {

            if (target.gameObject.activeInHierarchy) {
                DiedEvent.Invoke();
            }

            OnDeath();
        }
    }

    public virtual IEnumerator AttackCoroutine() {
        yield return new WaitForSeconds(0f);
    }

}
