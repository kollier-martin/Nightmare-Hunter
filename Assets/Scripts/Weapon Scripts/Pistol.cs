using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 0.3f;
        bullet.setDamage(5);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Shoot(3.0f);
    }
}
