using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private GameObject explosion = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Start()
    {
        rb.velocity = Vector2.left * 2.0f;

        StartCoroutine(FlashBoom());
    }

    IEnumerator FlashBoom()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Flash");
        yield return new WaitForSeconds(1.0f);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
