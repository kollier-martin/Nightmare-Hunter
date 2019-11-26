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
    protected SpriteRenderer SR;
    public Transform FirePoint;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    protected void Shoot(float bulletSpeed)
    {
        if (Time.timeScale == 1)
        {
            if (Input.GetButtonDown("Fire1") && (transform.parent.localScale.x > 0) && Time.time > nextFire || Input.GetAxis("Fire1") > 0 && (transform.parent.localScale.x > 0) && Time.time > nextFire)
            {
                nextFire = Time.time + shotDelay;
                bullet.setSpeedX(bulletSpeed);

                Instantiate(bullet, FirePoint.position, FirePoint.rotation);
                gunSound.Play();
            }
            else if (Input.GetButtonDown("Fire1") && (transform.parent.localScale.x < 0) && Time.time > nextFire || Input.GetAxis("Fire1") > 0 && (transform.parent.localScale.x < 0) && Time.time > nextFire)
            {
                nextFire = Time.time + shotDelay;
                bullet.setSpeedX(bulletSpeed * -1);

                Instantiate(bullet, FirePoint.position, FirePoint.rotation);
                gunSound.Play();
            }
        }
    }

    protected void Aim()
    {
        if (Time.timeScale == 1)
        {
            if (InputSwitch.KeyboardInput == true)
            {
                var pos = CameraSwitch.CurrentCam.WorldToScreenPoint(transform.position);
                var dir = Input.mousePosition - pos;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0) // Aiming left
                {
                    Vector3 newEulerAngles = transform.eulerAngles;
                    newEulerAngles.z += 180f;
                    transform.eulerAngles = newEulerAngles;
                }
            }
            else if (InputSwitch.JoyStickInput == true)
            {
                var JoystickPos = new Vector3(Input.GetAxis("RJoy X"), Input.GetAxis("RJoy Y"));
                var dir = new Vector3(JoystickPos.x, JoystickPos.y);
                var angle = Mathf.Atan2(-(dir.y), dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0) // Aiming left
                {
                    Vector3 newEulerAngles = transform.eulerAngles;
                    newEulerAngles.z += 180f;
                    transform.eulerAngles = newEulerAngles;
                }
            }
        }
    }
}
