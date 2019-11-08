using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioController : MonoBehaviour
{
    public enum State { Dead, Small, Big, Fire, Leaf, Frog};
    private State currentState = State.Small;

    [SerializeField]
    private float speedFactor;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private GameObject fireball;

    private Rigidbody2D rigidBody;
    private CapsuleCollider2D collider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float transitionStart = 0f;
    private float fireballStart = 0f;
    private bool doTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == State.Dead)
        {
            if(transform.position.y < -10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }
        if (doTransition)
        {
            Time.timeScale = 0;
            spriteRenderer.enabled = Time.realtimeSinceStartup % 0.1f< 0.05f;
            if(transitionStart+0.5f <= Time.realtimeSinceStartup)
            {
                doTransition = false;
                spriteRenderer.enabled = true;
                Time.timeScale = 1;
            }
        }
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.03f, 256);
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        float speedX = axisX * speedFactor;
        if(axisY < -0.05f && isGrounded)
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            SetCollider(State.Small);
            animator.SetBool("isCrouched", true);
        }
        else
        {
            animator.SetBool("isCrouched", false);
            SetCollider(currentState);
            rigidBody.velocity = new Vector2(speedX, rigidBody.velocity.y);
            if (Mathf.Abs(rigidBody.velocity.x) > 0.05)
            {
                spriteRenderer.flipX = rigidBody.velocity.x < 0;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidBody.AddForce(new Vector2(0, jumpForce));
            SoundManager.instance.PlaySound("Jump");
        }

        if (Input.GetButtonDown("Fire1") && currentState == State.Fire && fireballStart + 0.5f <= Time.realtimeSinceStartup)
        {
            if(spriteRenderer.flipX && fireball.GetComponent<FireballBehaviour>().speed > 0 ||
                !spriteRenderer.flipX && fireball.GetComponent<FireballBehaviour>().speed < 0)
            {
                fireball.GetComponent<FireballBehaviour>().speed *= -1;
            }
            Instantiate(fireball, transform);
            SoundManager.instance.PlaySound("Fireball");
            animator.SetTrigger("fireball");

            fireballStart = Time.realtimeSinceStartup;
        }

        animator.SetFloat("speed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool("isJumping", !isGrounded);
    }

    void StateBehaviour()
    {
        switch (currentState)
        {
            case State.Dead:
                break;
            case State.Small:
                break;
            case State.Big:
                break;
            case State.Fire:
                break;
            case State.Leaf:
                break;
            case State.Frog:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PowerUp>() != null)
        {
            ChangeState(collision.GetComponent<PowerUp>().behaviour);
            if(collision.transform.parent != null && collision.transform.parent.GetComponent<LeftRightBehaviour>() != null)
            {
                Destroy(collision.transform.parent.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }else if(collision.gameObject.tag == "DeathTrigger")
        {
            ChangeState(State.Dead);
        }
    }
    
    void ChangeState(State newState, bool forceState = false)
    {
        if(currentState == newState)
        {
            return;
        }
        if(!forceState && newState == State.Big && currentState != State.Small)
        {
            return;
        }
        switch (newState)
        {
            case State.Dead:
                animator.SetBool("isDead", true);
                
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<EnnemyBehaviour>().SetStatic(true);
                }
                SoundManager.instance.PlaySound("Death");
                break;
            case State.Small:
                animator.SetTrigger("triggerSmall");
                break;
            case State.Big:
                if(currentState == State.Small)
                {
                    SoundManager.instance.PlaySound("PowerUp");
                }
                animator.SetTrigger("triggerBig");
                break;
            case State.Fire:
                SoundManager.instance.PlaySound("PowerUp");
                animator.SetTrigger("triggerFire");
                break;
            case State.Leaf:
                SoundManager.instance.PlaySound("PowerUp");
                animator.SetTrigger("triggerLeaf");
                break;
            case State.Frog:
                SoundManager.instance.PlaySound("PowerUp");
                animator.SetTrigger("triggerFrog");
                break;
        }
        currentState = newState;
        SetCollider(currentState);
        doTransition = true;
        transitionStart = Time.realtimeSinceStartup;
    }

    private void SetCollider(State state)
    {
        switch (state)
        {
            case State.Dead:
                collider.enabled = false;
                rigidBody.velocity = new Vector2(0, 0);

                rigidBody.gravityScale = 0;

                StartCoroutine(ExecAfterTime(1.2f, () => {
                    rigidBody.gravityScale = 1;
                    rigidBody.AddForce(new Vector2(0, jumpForce*0.8f));
                    return true;
                }));

                break;
            case State.Small:
                collider.size = new Vector2(0.3f, 0.46f);
                collider.offset = new Vector2(-0.009f, 0.23f);
                break;
            case State.Big:
            case State.Fire:
            case State.Leaf:
            case State.Frog:
                collider.size = new Vector2(0.4f, 0.8f);
                collider.offset = new Vector2(-0.02f, 0.4f);
                break;
        }
    }

    public void GetDamage()
    {
        switch (currentState)
        {
            case State.Small:
                ChangeState(State.Dead);
                break;
            case State.Big:
                ChangeState(State.Small);
                break;
            case State.Fire:
            case State.Leaf:
            case State.Frog:
                ChangeState(State.Big, true);
                break;
        }
    }

    public void Bounce()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        if (Input.GetButton("Jump"))
        {
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }
        else
        {
            rigidBody.AddForce(new Vector2(0, 200));
        }
    }

    IEnumerator ExecAfterTime(float time, Func<bool> toExec)
    {
        yield return new WaitForSecondsRealtime(time);

        toExec.Invoke();
    }

}
