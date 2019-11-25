using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

abstract public class EnemyP : MonoBehaviour, MessageSystem
{
    protected readonly float moveSpeed = 1f;
    protected bool move = true;
    
    private float lastHitDone;

    private Animator anim;
    public GameObject DeathEffect;

    public Transform targetToHit;
    protected float targetDistance;

    public float MaxXPos, MinXPos;

    [SerializeField] protected SpriteRenderer SP;
    [SerializeField] protected float myHealth;
    protected float speed;
    protected float attackRange;
    protected int damage;
    protected float attackDelay;

    public void Awake()
    {
        SP = GetComponent<SpriteRenderer>();
        setObjects();
    }

    public virtual void Update()
    {
        // If health below 1, entity is dead
        if (myHealth <= 0f)
        {
            // Activate death animation then Destroy object
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        // Move entity in a path
        Move();

        // If close enough, attack damage
        if (targetDistance < attackDelay)
        {
            // Use Trigger on collider as a vision to start attacking the player

        }
    }

    void Move()
    {
        if (myHealth > 0)
        {
            if (transform.position.x > MaxXPos)
            {
                move = false;
                SP.flipX = true;
            }

            if (transform.position.x < MinXPos)
            {
                move = true;
                SP.flipX = false;
            }

            if (move)
            {
                transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else if (!move)
            {
                transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }
    }

    protected void setObjects()
    {
        anim = GetComponent<Animator>();
        targetDistance = Vector3.Distance(transform.position, targetToHit.position);
    }
    protected void SetVariables(int damage, int myHealth, int speed, float attackRange, float attackDelay)
    {
        this.damage = damage;
        this.myHealth = myHealth;
        this.speed = speed;
        this.attackRange = attackRange;
        this.attackDelay = attackDelay;
    }

    public void Die()
    {
        myHealth = 0;
    }

    public void SpawnHere(GameObject Spawn)
    {
        transform.position = Spawn.transform.position;
    }

    public void TakeDamage(int damage)
    {
        myHealth -= damage;
    }
}
