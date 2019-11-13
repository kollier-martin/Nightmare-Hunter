using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Grog : EnemyP
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider health;
    [SerializeField] private GameObject portal;
    [SerializeField] private readonly string myName = "Grog";

    // Start is called before the first frame update
    void Start()
    {
        SetVariables(30, 500, 10, 1.0f, 1.0f);
        setObjects();

        // Distance between entity and player
        targetDistance = Vector3.Distance(transform.position, targetToHit.position);
    }

    public override void Update()
    {
        text.text = myName;
        health.value = myHealth / 100.0f;

        // If health below 1, entity is dead
        if (myHealth < 1.0f)
        {
            Destroy(gameObject);
            BossIsDead();
        }
    }

    void BossIsDead()
    {
        // Call specific cutscene depending on which Boss dies
        // Play cutscene


        // On Boss death spawn next level portal
        Instantiate(platform, new Vector3(21.85f, -0.57f, 1), Quaternion.identity);
        Instantiate(portal);
    }
}
