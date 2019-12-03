using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackCollider : MonoBehaviour, IEventSystemHandler
{
    public GameObject Explosion;
    private Rigidbody2D rb;
    public Transform targetToHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector3 dir = targetToHit.transform.position - transform.position;
        dir = targetToHit.transform.TransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 20);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile forward towards the player's last known direction;
        transform.position += transform.right * 14f * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            ExecuteEvents.Execute<Player>(collision.collider.gameObject, null, (x, y) => x.TakeDamage(10));
            Instantiate(Explosion, collision.collider.transform.position, collision.collider.transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.collider.name == "Marlo" || collision.collider.tag == "BossAttack")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
