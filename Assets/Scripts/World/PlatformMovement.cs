using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float MaxX, MaxY, MinX, MinY;
    public float moveSpeed = 2.5f;
    bool moveY = true;
    bool moveX = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (name.Contains("PX"))
       {
            MoveX();
       }
       else if (name.Contains("PY"))
       {
            MoveY();
       }
    }

    void MoveX()
    {
        if (transform.position.x > MaxX)
        {
            moveX = false;
        }

        if (transform.position.x < MinX)
        {
            moveX = true;
        }

        if (moveX)
        {
            rb.MovePosition(new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y));
        }

        else if (!moveX)
        {
            rb.MovePosition(new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y));
        }
    }

    void MoveY()
    {
        if (transform.position.y > MaxY)
        {
            moveY = false;
        }

        if (transform.position.y < MinY)
        {
            moveY = true;
        }

        if (moveY)
        {
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime));
        }

        else if (!moveY)
        {
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime));
        }
    }
}
