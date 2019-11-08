using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{

    [SerializeField]
    public float speed = 0;
    [SerializeField]
    private float bounceForce = 0;
    private Rigidbody2D rigidBody;
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(collision.contactCount / 2);

        if (collision.gameObject.tag == "Enemy")
        {

            MonoBehaviour[] monob = collision.gameObject.GetComponents<MonoBehaviour>();
            Collider2D enemyCollider = collision.gameObject.GetComponent<Collider2D>();
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            foreach (MonoBehaviour m in monob)
            {
                if (m.GetType() != this.GetType())
                {
                    m.enabled = false;
                }
            }

            enemyCollider.enabled = false;
            enemyRigidbody.gravityScale = 1f;

            enemyRigidbody.velocity = new Vector2(0, 0);
            enemyRigidbody.AddForce(new Vector2(speed*10, 200));

            collision.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            collision.gameObject.GetComponent<Animator>().enabled = false;

            Destroy(collision.gameObject, 3f);
            Destroy(gameObject);
        }
        else if (Physics2D.Raycast(transform.position, Vector2.down, 0.03f, 256))
        {
            rigidBody.AddForce(new Vector2(0, bounceForce));
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
