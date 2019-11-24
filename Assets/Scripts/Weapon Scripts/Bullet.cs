using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    public GameObject DestroyEffect;
    public Rigidbody2D rb;

    private void Start()
    {
        rb.velocity = transform.right * speedX;

        if (name == "Snub Bullet")
        {
            Destroy(gameObject, 3);
        }
        else
        {
            Destroy(gameObject, 4);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ExecuteEvents.Execute<MessageSystem>(collision.gameObject, null, (x, y) => x.TakeDamage(damage));
        }

        Instantiate(DestroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void setDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void setSpeedX(float speed)
    {
        speedX = speed;
    }
}
