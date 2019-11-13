using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private Transform Holder;

    // Update is called once per frame
    void Update()
    {
        if(enemy != null)
        {
            //Instantiate(enemy, Holder.transform);
        }
    }
}
