using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class IGun : MonoBehaviour
{
    [SerializeField] private AudioSource gunSound;
    public Bullet bullet;
    protected float shotDelay;
    protected float nextFire = 0.0f;


    protected void Shoot(float bulletSpeed)
    {
        if (Input.GetButtonDown("Fire1") && (transform.parent.localScale.x > 0) && Time.time > nextFire || Input.GetAxis("Fire1") > 0 && (transform.parent.localScale.x > 0) && Time.time > nextFire)
        {
            nextFire = Time.time + shotDelay;
            bullet.setSpeedX(bulletSpeed);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -90));
            GetComponentInChildren<AudioSource>().Play();
        }
        else if (Input.GetButtonDown("Fire1") && (transform.parent.localScale.x < 0) && Time.time > nextFire || Input.GetAxis("Fire1") > 0 && (transform.parent.localScale.x < 0) && Time.time > nextFire)
        {
            nextFire = Time.time + shotDelay;
            bullet.setSpeedX(bulletSpeed * -1);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90));
            GetComponentInChildren<AudioSource>().Play();
        }
    }

    /*protected void Aim()
    {
        //  Aim gun with mouse and have bullet follow it also
        //  Get the vector pointing at the mouse relative to the gun
        var relVec = Input.mousePosition - transform.position;

        //  Normalize the vector so it has a length of 1, then scale by the speed of your bullet
        //  and add that as force to the bullets rigidbody
        bullet.GetComponent<Rigidbody2D>().AddForce(relVec.Normalize() * bulletSpeed);

        //  That should be all that's needed for the bullet traveling toward the mouse
        //  TO rotate the gun convert the relativeVector to an angle in radians using Math.Atan2(float y, float x)
        var zAngle = System.Math.Atan2(relVec.y, relVec.x);

        //Set the z rotation of the gun to be that angle without changing the z or y angles
        var angles = transform.rotation.eulerAngles;

        transform.rotation.eulerAngles = new Vector3(angles.x, angles.y, zAngle);

        //That should make it rotate but you might also want it to flip on the y axis so the gun doesnt rotate until it's upside down. 
        //If the z Angle is in the left hemisphere of the unit circle, -Pi/2 to Pi/2 it should face right. The easiest way to test this is to take the cosine of the angle, which will be positive if the angle is to the right of the player, and negative if it's to the left
        if (System.Math.Cos(zAngle) > 0)
        {
            //Face right

        }

        else
        {
            //Face left

        }

    }*/
}
