using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 0.7f;
        if(name.Contains("Upgraded"))
        {
            bullet.setDamage(22);
        }
        else
        {
            bullet.setDamage(16);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Shoot(3.0f);
    }
}
