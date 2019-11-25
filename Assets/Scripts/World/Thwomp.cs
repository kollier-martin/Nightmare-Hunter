using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Thwomp : MonoBehaviour
{
    AudioSource MySound;
    GameObject Cam;
    Rigidbody2D MyRB;

    private const float fallSpeed = -235;
    private const float riseSpeed = 5.0f;
    private bool PlayerUnder = false;
    public bool OnGround = false;

    private float ShakeSpeed, ShakeDuration;

    Quaternion originalRotation;
    [SerializeField] Vector3 originalTrans;
    Vector3 tempPos;

    float ShakeDecrease;

    // Start is called before the first frame update
    void Start()
    {
        Cam = FindObjectOfType<Camera>().gameObject;

        MySound = GetComponent<AudioSource>();
        MyRB = GetComponent<Rigidbody2D>();

        originalTrans = transform.position;
        originalRotation = transform.rotation;

        ShakeSpeed = 0.1f;
        ShakeDuration = 2.0f;
        ShakeDecrease = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerUnder == true && transform.position.y == originalTrans.y)
        {
            StartCoroutine(ShakeToDrop());
        }

        Rise();
    }

    IEnumerator ShakeToDrop()
    {
        MySound.Play();

        float counter = 0f;

        //Angle Rotation(Optional)
        const float angleRot = 4;

        //Do the actual shaking
        while (counter < ShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed = ShakeSpeed;
            float decreaseAngle = angleRot;

            //Shake GameObject
            
            //Don't Translate the Z Axis if 2D Object
            tempPos = originalTrans + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
            tempPos.z = originalTrans.z;
            transform.position = tempPos;

            //Only Rotate the Z axis if 2D
            transform.rotation = originalRotation * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            
            yield return null;


            //Check if we have reached the decreasePoint then start decreasing decreaseSpeed value
            if (counter >= ShakeDecrease)
            {
                //Reset counter to 0 
                counter = 0f;
                while (counter <= ShakeDecrease)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(ShakeSpeed, 0, counter / ShakeDecrease);
                    decreaseAngle = Mathf.Lerp(angleRot, 0, counter / ShakeDecrease);
                    
                    //Shake GameObject

                    //Don't Translate the Z Axis if 2D Object
                    tempPos = originalTrans + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                    tempPos.z = originalTrans.z;
                    transform.position = tempPos;

                    //Only Rotate the Z axis if 2D
                    transform.rotation = originalRotation * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                    
                    yield return null;
                }

                //Break from the outer loop
                break;
            }
        }

        // Drops the sprite
        MyRB.AddForce(transform.up * fallSpeed);
    }

    void Rise()
    {
        // If the Block has hit ground, rise it back to it's original state
        if (OnGround)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalTrans, riseSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetType() == typeof (BoxCollider2D))
        {
            StartCoroutine(ShakeToDrop());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerUnder = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            ExecuteEvents.Execute<CameraFollow>(Cam, null, (x, y) => x.Shake());
            OnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            OnGround = false;
        }
    }
}
