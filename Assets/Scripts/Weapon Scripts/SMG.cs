using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 0.4f;

        if (name.Contains("Upgraded"))
        {
            bullet.setDamage(14);
        } 
        else
        {
            bullet.setDamage(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Shoot(3.0f);
    }
}
