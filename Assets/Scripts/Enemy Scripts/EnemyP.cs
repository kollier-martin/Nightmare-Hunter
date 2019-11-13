using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyP : MonoBehaviour, MessageSystem
{
    public GameObject platform;
    float moveSpeed = 0f;
    bool move = true;

    private float lastHitDone;
    private Animator anim;

    public Transform targetToHit;
    protected float targetDistance;

    float MaxXPos, MinXPos;

    [SerializeField] protected float myHealth;
    protected float speed;
    protected float attackRange;
    protected int damage;
    protected float attackDelay;

    public void Awake()
    {
        setObjects();
    }

    public virtual void Update()
    {
        // If health below 1, entity is dead
        if (myHealth < 1.0f)
        {
            // Activate death animation
            // At end of death animation run, then destroy
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if (transform.position.x > MaxXPos)
        {
            move = false;
        }

        if (transform.position.x < MinXPos)
        {
            move = true;
        }

        if (move)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }else if (!move)
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
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

    protected void SetMaxMin(float min, float max)
    {
        MaxXPos = max;
        MinXPos = min;
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
