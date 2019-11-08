using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class EnnemyBehaviour : MonoBehaviour
{

    protected Animator animator;
    protected Collider2D collider;
    protected Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetStatic(bool isStatic)
    {
        if (isStatic)
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
            animator.enabled = false;
        }
        else
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.GetContact(collision.contactCount / 2).normal.y);
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetContact(collision.contactCount / 2).normal.y < -0.5)
            {
                animator.SetBool("isDead", true);

                MonoBehaviour[] monob = GetComponents<MonoBehaviour>();
                Collider2D collider = GetComponent<Collider2D>();
                foreach (MonoBehaviour m in monob)
                {
                    if (m.GetType() != this.GetType())
                    {
                        m.enabled = false;
                    }
                }

                collider.enabled = false;
                rigidbody.gravityScale = 0;

                rigidbody.velocity = new Vector2(0, 0);
                collision.gameObject.GetComponent<MarioController>().Bounce();
                Destroy(gameObject, 0.5f);
            }
            else
            {
                collision.gameObject.GetComponent<MarioController>().GetDamage();
            }
        }
    }
}
