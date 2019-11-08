using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class KoopaBehaviour : EnnemyBehaviour
{

    LeftRightBehaviour leftRightBehaviour;
    private SpriteRenderer spriteRenderer;

    bool shell = false;
    float baseSpeed;
    [SerializeField]
    float shellSpeed = 0;

    [SerializeField]
    bool flying = false;
    [SerializeField]
    float flyingSpeed = 0;
    Vector2 basePos;
    Vector2 targetPos;
    Vector2 lastPos;
    bool goToTarget = false;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        leftRightBehaviour = GetComponent<LeftRightBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSpeed = leftRightBehaviour.speed;
        if (flying)
        {
            basePos = transform.position;
            Transform target = transform.Find("Target");
            targetPos = target.position;
            rigidbody.gravityScale = 0;
            leftRightBehaviour.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            if (goToTarget)
            {
                rigidbody.MovePosition(Vector2.MoveTowards(transform.position, targetPos, flyingSpeed * Time.deltaTime));
                if(transform.position == new Vector3(targetPos.x, targetPos.y, transform.position.z))
                {
                    goToTarget = false;
                }
            }
            else
            {
                rigidbody.MovePosition(Vector2.MoveTowards(transform.position, basePos, flyingSpeed * Time.deltaTime));
                if (transform.position == new Vector3(basePos.x, basePos.y, transform.position.z))
                {
                    goToTarget = true;
                }
            }
        }
        Vector2 trackVelocity = (rigidbody.position - lastPos)/Time.deltaTime;
        lastPos = rigidbody.position;
        if (Mathf.Abs(trackVelocity.x) > 0.1)
        {
            spriteRenderer.flipX = trackVelocity.x > 0;
            Debug.Log(rigidbody.velocity.x);
        }
        animator.SetFloat("speed", Mathf.Abs(rigidbody.velocity.x));
        animator.SetBool("flying", flying);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.GetContact(collision.contactCount / 2).normal.y);
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetContact(collision.contactCount / 2).normal.y < -0.05)
            {
                if (flying)
                {
                    flying = false;
                    rigidbody.gravityScale = 1f;
                    leftRightBehaviour.enabled = true;
                }
                else
                {
                    if (!shell)
                    {
                        shell = true;
                        animator.SetBool("shell", shell);
                        gameObject.layer = LayerMask.NameToLayer("Projectile");
                        ((BoxCollider2D)collider).size = new Vector2(0.5f, 0.5f);
                        collider.offset = new Vector2(0, 0.25f);

                        leftRightBehaviour.speed = 0;
                    }
                    else
                    {
                        if (leftRightBehaviour.speed == 0)
                        {
                            if (collision.gameObject.GetComponent<SpriteRenderer>().flipX)
                            {
                                leftRightBehaviour.speed = -shellSpeed;
                            }
                            else
                            {
                                leftRightBehaviour.speed = shellSpeed;
                            }
                            SoundManager.instance.PlaySound("Bump");
                        }
                        else
                        {
                            leftRightBehaviour.speed = 0;
                        }
                    }
                }
                collision.gameObject.GetComponent<MarioController>().Bounce();
            }
            else
            {
                if (shell)
                {
                    if (leftRightBehaviour.speed == 0)
                    {
                        if (collision.gameObject.GetComponent<SpriteRenderer>().flipX)
                        {
                            leftRightBehaviour.speed = -shellSpeed;
                        }
                        else
                        {
                            leftRightBehaviour.speed = shellSpeed;
                        }
                        SoundManager.instance.PlaySound("Bump");
                    }
                    else
                    {
                        collision.gameObject.GetComponent<MarioController>().GetDamage();
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<MarioController>().GetDamage();
                }
            }
        }
        else if (collision.gameObject.tag == "Enemy") {

            collision.gameObject.GetComponent<Animator>().SetBool("isDead", true);

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
            enemyRigidbody.gravityScale = 0;

            enemyRigidbody.velocity = new Vector2(0, 0);
            Destroy(collision.gameObject, 0.5f);
        }
        else
        {
            ContactPoint2D contact = collision.GetContact(collision.contactCount / 2);
            if (shell && Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y))
            {
                SoundManager.instance.PlaySound("Bump");
            }
        }
    }

    public bool GetShellState()
    {
        return shell;
    }
}
