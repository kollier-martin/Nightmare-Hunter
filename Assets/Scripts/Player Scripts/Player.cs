using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, MessageSystem
{
    private Slider health;
    private bool dead = false;
    int i = 0;

    private Animator anim;
    private Rigidbody2D rb;

    private GameObject GameSpawn;
    private SpriteRenderer CanInteractSR;
    private SpriteRenderer PlayerGotSR;
    private Animator PlayerGotAnim;
    private GameController CurrentController;

    [SerializeField] private ParticleSystem DeathParticle;
    [SerializeField] private GameObject PlayerGot;
    [SerializeField] private GameObject CanInteract;
    [SerializeField] private GameObject Gun;
    [SerializeField] private List<GameObject> GunsOwned;
    [SerializeField] private List<GameObject> ItemInventory;

    public PlayerController myController;

    private bool HasKey = false;
    private float horizontal = 0f;
    private const int speed = 15;
    private bool jump = false;

    [SerializeField] bool climb;
    public bool onPortal = false;

    private const float positionAdd = 0.20f;
    public static Vector2 OriginalScale;

    public static float myHealth = 100.0f;
    public static string myName = "Seji";

    private int CurrentGunIndex = 0;
    public bool bossIsDead = false;
    private bool UnderBlock = false;

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        bossIsDead = false;
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;

        CurrentController = FindObjectOfType<GameController>();

        GameSpawn = GameObject.FindGameObjectWithTag("Respawn");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        CanInteractSR = CanInteract.GetComponent<SpriteRenderer>();
        PlayerGotSR = PlayerGot.GetComponent<SpriteRenderer>();
        PlayerGotAnim = PlayerGot.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Gun = Instantiate(Gun, new Vector3(transform.position.x + positionAdd, transform.position.y, transform.position.z), transform.rotation, transform);
        }
        catch (System.Exception)
        {

        }

        OriginalScale = transform.localScale;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;
        
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetFloat("Jump", 1);
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            anim.SetFloat("Jump", 0);
        }

        foreach (GameObject item in ItemInventory)
        {
            if (item.name == "Key")
            {
                HasKey = true;
            }
        }

        if (dead == true)
        {
            StartCoroutine(Death());
            dead = false;
        }

        if (health != null)
        {
            health.value = myHealth / 100;
        }

        if (HasKey == true && Input.GetButtonDown("Open") && bossIsDead == true && i < 1 && onPortal == true)
        {
            ItemInventory.Remove(GameObject.FindWithTag("Key"));
            HasKey = false;
            ExecuteEvents.Execute<GameController>(CurrentController.gameObject, null, (x, y) => x.LoadNextScene());
            i++;
        }
        else if (HasKey == true && Input.GetButtonDown("Open") && bossIsDead == false && i < 1 && onPortal == true)
        {
            StartFight();
            i++;
        }

        SwapGun();
        i = 0;
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
        if(Time.timeScale == 1)
        {
            if (GunsOwned != null)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("DPadX") == 1)
                {
                    if (CurrentGunIndex == (GunsOwned.Count - 1))
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
                    if (CurrentGunIndex == 0 && GunsOwned.Count > 1 && CurrentGunIndex < GunsOwned.Count)
                    {
                        CurrentGunIndex += 1;
                        Swap(CurrentGunIndex);
                    }
                    else
                    {
                        CurrentGunIndex -= 0;
                        Swap(CurrentGunIndex);
                    }
                }
            }
        }
    }

    private void Swap(int index)
    {
        Destroy(Gun);
        Gun = Instantiate(GunsOwned[index], new Vector3(transform.position.x + positionAdd, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
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
                rb.velocity += collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                break;
        }

        if (collision.gameObject.tag == "Crusher" && UnderBlock == true)
        {
            TakeDamage(15);
            StartCoroutine(Smush());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Moving Platform")
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case ("Ladders"):
                climb = true;
                jump = false;
                break;

            case ("Crusher"):
                UnderBlock = true;
                break;


            case ("Portal"):
                onPortal = true;
                break;
        }

        if (other.gameObject.layer == 10)
        {
            CanInteract.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.name)
        {
            case ("Ladders"):
                climb = false;
                jump = true;
                break;

            case ("Falling Block"):
                UnderBlock = false;
                break;
        }

        if (other.gameObject.layer == 10)
        {
            CanInteract.SetActive(false);
        }
    }

    public void StartFight()
    {
        var BossSpawn = GameObject.FindGameObjectWithTag("Boss Spawn");
        SpawnHere(BossSpawn);

        ExecuteEvents.Execute<GameController>(CurrentController.gameObject, null, (x, y) => x.InstantiateMarlo());
    }

    IEnumerator Death()
    {
        Gun.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        DeathParticle.Play();
        yield return new WaitForSeconds(1.5f);
        tag = "Dead";
    }

    IEnumerator Smush()
    {
        // Makes the "y" scale smaller and stores the "x" scale before being hit
        OriginalScale.x = transform.localScale.x;
        OriginalScale.y -= 0.80f;
        transform.localScale = OriginalScale;

        yield return new WaitForSeconds(1.0f);

        // Increase the "y" to it's original state and store the "x' before reverting back to normal
        OriginalScale.x = transform.localScale.x;
        OriginalScale.y += 0.80f;
        transform.localScale = OriginalScale;
    }

    public void Die()
    {
        myHealth = 0;
        SendData();
        dead = true;
    }

    public void SpawnHere(GameObject Spawn)
    {
        transform.position = new Vector3(Spawn.transform.position.x, Spawn.transform.position.y, Spawn.transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        myHealth -= damage;
    }

    public void GetItem(GameObject Item)
    {
        if (Item.tag == "Gun")
        {
            GunsOwned.Add(Item);
        }
        else
        {
            ItemInventory.Add(Item);
        }

        StartCoroutine(DisplayItem(2.0f, Item));
    }

    IEnumerator DisplayItem(float TimeToDelay, GameObject Item)
    {
        PlayerGotSR.GetComponent<SpriteRenderer>().sprite = Item.GetComponent<SpriteRenderer>().sprite;

        yield return new WaitForSeconds(TimeToDelay);

        PlayerGotSR.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void SendData()
    {
        // Send current data to GameController
        ExecuteEvents.Execute<GameController>(CurrentController.gameObject, null, (x, y) => x.ProcessPlayerData(transform.position, GunsOwned, ItemInventory, SceneManager.GetActiveScene()));
    }

    public void DestroyMyself()
    {
        Destroy(gameObject);
    }
}
