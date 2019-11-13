using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Holder;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            // Start cutscene
            Instantiate(Boss, Holder.transform);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
