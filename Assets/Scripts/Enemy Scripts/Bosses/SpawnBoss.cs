using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnBoss : MonoBehaviour, IEventSystemHandler
{
    public GameObject Boss;
    public GameObject Holder;

    public void PlaceBoss()
    {
        // Marlo Handler
        Instantiate(Boss, new Vector3(Holder.transform.position.x, Holder.transform.position.y, Holder.transform.position.z), Holder.transform.rotation, Holder.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Grog Conditionals
        if (collision.isTrigger && collision.tag == "Player" && Boss.name == "Grog") 
        {
            Instantiate(Boss, Holder.transform);

            try
            {
                GetComponent<Collider2D>().enabled = false;
            }
            catch (Exception)
            {

            }
        }
    }
}
