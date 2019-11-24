using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skull : EnemyP
{
    public float MinDist, MaxDist;

    // Start is called before the first frame update
    void Start()
    {
        MinDist = 0; MaxDist = 10;
        SetVariables(7, 100, 10, 1, 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Vector3.Distance(transform.position, targetToHit.position) >= MinDist && Vector3.Distance(transform.position, targetToHit.position) < MinXPos && Vector3.Distance(transform.position, targetToHit.position) > MaxXPos)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        // If health below 1, entity is dead
        if (myHealth <= 0f)
        {
            // Activate death animation then Destroy object
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ExecuteEvents.Execute<MessageSystem>(collision.gameObject, null, (x, y) => x.TakeDamage(5));
        }
    }
}
