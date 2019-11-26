using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 1.0f;

        if (name.Contains("Snub"))
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
