using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, MessageSystem
{
    public Slider health;

    private Rigidbody2D rb;
    private Animator anim;

    private GameObject GameSpawn;
    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject[] GunsOwned;

    public PlayerController myController;

    float horizontal = 0f;
    public int speed = 10;
    bool jump = false;
    [SerializeField] bool climb;

    const float positionAdd = 0.20f;

    public float myHealth = 100.0f;

    private int CurrentGunIndex = 0;

    [SerializeField] private bool dead = false;

    private void Awake()
    {
        GameSpawn = GameObject.FindGameObjectWithTag("Respawn");
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Gun = Instantiate(Gun, new Vector3(transform.position.x + positionAdd, transform.position.y, transform.position.z - .25f), transform.rotation, transform.parent);
        Gun.transform.parent = transform;

        anim = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        
        health.value = myHealth / 100;
        
        SwapGun();
        AmIDead();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myController.Move(horizontal * Time.fixedDeltaTime, climb, jump);
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));

        jump = false;
    }
    
    private void SwapGun()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("DPadX") == 1)
        {
            if (CurrentGunIndex == (GunsOwned.Length - 1))
            {
                CurrentGunIndex = 0;
                Swap(CurrentGunIndex);
            }
            else
            {
                CurrentGunIndex += 1;
                Swap(CurrentGunIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("DPadX") == -1)
        {
            if (CurrentGunIndex == 0)
            {
                CurrentGunIndex = 4;
                Swap(CurrentGunIndex);
            }
            else
            {
                CurrentGunIndex -= 1;
                Swap(CurrentGunIndex);
            }
        }
    }

    private void Swap(int index)
    {
        Destroy(Gun);
        Gun = Instantiate(GunsOwned[index], new Vector3(transform.position.x + positionAdd, transform.position.y, transform.position.z - .25f), transform.rotation, transform.parent);
        Gun.transform.parent = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Switch Case Here
        switch (collision.gameObject.tag)
        {
            case ("Boundary"):
                // Take fall damage
                TakeDamage(5);

                // Respawn
                transform.position = GameSpawn.transform.position;
                break;

            case ("Moving Platform"):
                rb.velocity *= collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.name)
        {
            case ("Ladders"):
                climb = true;
                jump = false;
                break;

            case ("Portal"):
                Debug.Log("Loading Scene...");
                ExecuteEvents.Execute<GameController>(FindObjectOfType<GameController>().gameObject, null, (x, y) => x.LoadNextScene());
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Ladders")
        {
            climb = false;
            jump = true;
        }
    }

    public void DeadPlayer()
    {
        Destroy(this);
    }
    
    private void AmIDead()
    {
        if (myHealth <= 0)
        {
            dead = true;
            anim.SetBool("Dead", true);
        }
        else if (myHealth > 0)
        {
            dead = false;
        }
    }

    public void Die()
    {
        myHealth = 0;
        dead = true;
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
