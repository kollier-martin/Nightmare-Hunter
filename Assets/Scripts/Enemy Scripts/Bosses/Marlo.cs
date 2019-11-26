using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using System.Linq;

public class Marlo : MonoBehaviour, IEventSystemHandler, MessageSystem
{
    [SerializeField]
    private List<GameObject> Attacks = new List<GameObject>();

    [SerializeField]
    private Transform AttackSpawner;

    private GameController CurrenController;
    public GameObject Portal;
    
    private bool canAttack;
    public float myHealth;

    public GameObject PDialog;

    private float attackTime;
    private float coolDown;

    [SerializeField] private SpriteRenderer SP;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem DeathParticle;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider health;
    [SerializeField] private Transform targetToHit;
    [SerializeField] private float targetDistance;
    [SerializeField] private readonly string myName = "Marlo";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SP = GetComponent<SpriteRenderer>();
        CurrenController = FindObjectOfType<GameController>();

        canAttack = false;

        myHealth = 300f;
        attackTime = 1.0f;
        coolDown = 2.0f;

        // Distance between entity and player
        if (targetToHit != null)
        {
            targetDistance = Vector3.Distance(transform.position, targetToHit.position);
        }
    }

    public void Update()
    {
        text.text = myName;
        health.value = myHealth / 100.0f;

        // If health below 1, entity is dead
        if (myHealth <= 0f)
        {
            StartCoroutine(IAmDead());
        }

        if (CurrenController.cutsceneDone == true)
        {
            canAttack = true;
        }

        Attack();
    }

    IEnumerator IAmDead()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        DeathParticle.Play();

        yield return new WaitForSeconds(DeathParticle.GetComponent<ParticleSystem>().main.duration);
        ExecuteEvents.Execute<GameController>(CurrenController.gameObject, null, (x, y) => x.BossIsDead());
        Instantiate(Portal, transform.position, transform.rotation);
        PDialog.SetActive(true);
        Destroy(gameObject);
    }

    void Attack()
    {
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        if (attackTime < 0)
        {
            attackTime = 0;
        }

        if (canAttack && attackTime == 0)
        {
            var rand = Random.Range(0, Attacks.Count);
            Instantiate(Attacks[rand], AttackSpawner.transform.position, Quaternion.identity, AttackSpawner.transform);
            attackTime = coolDown;
        }
    }

    public void Die()
    {
        myHealth = 0;
    }

    public void SpawnHere(GameObject Spawn)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        myHealth -= damage;
    }
}
