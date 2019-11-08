using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopPowerUp : MonoBehaviour
{
    Vector3 target = new Vector3(0, 0.5f, 0);
    MonoBehaviour[] monob;
    Collider2D[] colliders;
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        monob = GetComponents<MonoBehaviour>();
        colliders = GetComponents<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        foreach(MonoBehaviour m in monob)
        {
            if(m.GetType() != this.GetType())
            {
                m.enabled = false;
            }
        }
        foreach (Collider2D c in colliders)
        {
            if (!c.isTrigger)
            {
                c.enabled = false;
            }
        }
        if(rigidbody)
        {
            rigidbody.gravityScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(target, transform.localPosition) < 0.05f)
        {
            foreach (MonoBehaviour m in monob)
            {
                if (m.GetType() != this.GetType())
                {
                    m.enabled = true;
                }
            }
            foreach (Collider2D c in colliders)
            {
                if (!c.isTrigger)
                {
                    c.enabled = true;
                }
            }
            if (rigidbody)
            {
                rigidbody.gravityScale = 1;
            }
            this.enabled = false;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, 0.07f);
    }
}
