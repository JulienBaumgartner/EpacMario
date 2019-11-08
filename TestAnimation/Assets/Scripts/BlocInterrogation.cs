using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BlocInterrogation : MonoBehaviour
{
    bool isOpen = false;
    Animator animator;

    [SerializeField]
    List<GameObject> powerUps;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isOpen && ((collision.gameObject.tag == "Player" && collision.GetContact(collision.contactCount / 2).normal.y > 0.5) ||
            (collision.gameObject.GetComponent<KoopaBehaviour>() != null && collision.gameObject.GetComponent<KoopaBehaviour>().GetShellState()
            && System.Math.Abs(collision.GetContact(collision.contactCount / 2).normal.x) > 0.5)))
        {
            isOpen = true;
            animator.SetBool("isOpen", true);
            GameObject powerUp = powerUps[Random.Range(0, powerUps.Count - 1)];
            Instantiate(powerUp, transform);
        }
    }
}
