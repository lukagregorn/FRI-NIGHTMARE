using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private float knockTime;
    private float knockThrust;
    private int damage;

    public float moveSpeed;
    public Vector2 directionToMove;
    public float lifetime;
    private float lifetimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        lifetimeSeconds = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeSeconds -= Time.deltaTime;
        if(lifetimeSeconds <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() {
        // rotate me
        Vector3 v = new Vector3(0.0f,0.0f, 1.0f);
        transform.Rotate(v, 90 * Time.deltaTime);    
    }

    public void Launch(Vector3 initialVel, int d, float t, float kt)
    {
        damage = d;
        knockTime = t;
        knockThrust = kt;

        Vector2 vel = initialVel.normalized * moveSpeed;
        StartCoroutine(FireCoroutine(vel));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // check if player
        if (other.gameObject.CompareTag("Player") && other.isTrigger) {

            // get humanoid object
            Humanoid playerHuman = other.GetComponent<Humanoid>();
            
            // calculate knock vector
            Vector2 knockVector = other.GetComponent<Rigidbody2D>().transform.position - transform.position;
            knockVector = knockVector.normalized * knockThrust;

            // inflict knockback and damage
            playerHuman.TakeDamage(damage);
            playerHuman.Knockback(knockVector, knockTime);

            // destroy self
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FireCoroutine(Vector2 vel) {
        while (myRigidbody == null)
            yield return new WaitForSeconds(0f);

        myRigidbody.velocity = vel;
    }
}