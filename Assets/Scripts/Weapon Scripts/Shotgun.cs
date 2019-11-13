using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : IGun
{
    // Start is called before the first frame update
    void Start()
    {
        shotDelay = 1.5f;
        bullet.setDamage(30);
    }

    // Update is called once per frame
    void Update()
    {
        base.Shoot(3.0f);
    }

    public new void Shoot(float shotSpeed)
    {
        // Shoot pellets
    }
}
