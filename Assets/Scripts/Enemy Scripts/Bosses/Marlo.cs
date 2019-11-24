using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Marlo : EnemyP, IEventSystemHandler
{
    private GameController CurrenController;
    public GameObject Portal;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider health;
    [SerializeField] private readonly string myName = "Marlo";

    // Start is called before the first frame update
    void Start()
    {
        CurrenController = FindObjectOfType<GameController>();
        SetVariables(30, 300, 10, 1.0f, 1.0f);
        setObjects();

        // Distance between entity and player
        targetDistance = Vector3.Distance(transform.position, targetToHit.position);
    }

    public override void Update()
    {
        text.text = myName;
        health.value = myHealth / 100.0f;

        // If health below 1, entity is dead
        if (myHealth <= 0f)
        {
            IAmDead();
        }
    }

    void IAmDead()
    {
        // Play cutscene
        ExecuteEvents.Execute<GameController>(CurrenController.gameObject, null, (x, y) => x.BossIsDead());
        Instantiate(DeathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
