using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 3.0f;
        if (name.Contains("Upgraded"))
        {
            bullet.setDamage(40);
        }
        else
        {
            bullet.setDamage(30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shoot(3.0f);
    }
}
