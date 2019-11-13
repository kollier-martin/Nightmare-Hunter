using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speedX;
    private readonly float speedY = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
         GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);

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
        /* For shotgun handling
         * if (collision.gameObject.tag == "Bullet")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collision2D>().collider);
        }
        else*/

        if (collision.gameObject.tag == "Enemy")
        {
            ExecuteEvents.Execute<MessageSystem>(collision.gameObject, null, (x, y) => x.TakeDamage(damage));
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
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
