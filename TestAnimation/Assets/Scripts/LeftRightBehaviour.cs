using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LeftRightBehaviour : MonoBehaviour
{
    [SerializeField]
    public float speed = 0;
    private Rigidbody2D rigidBody;
    private Collider2D collider;
    private bool run = false;
    
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

    public void AnimFinished()
    {
        run = true;
        collider.enabled = true;
        transform.localPosition = new Vector3(0, 1f, 0);
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(collision.contactCount / 2);
        if (Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y) && collision.gameObject.tag != "Enemy")
        {
            speed *= -1;
        }
    }
}
