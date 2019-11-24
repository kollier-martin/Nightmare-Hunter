using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float MaxX, MaxY, MinX, MinY;
    readonly float moveSpeed = 2.5f;
    bool moveY = true;
    bool moveX = true;

    // Update is called once per frame
    void Update()
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
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }

        else if (!moveX)
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
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
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        }

        else if (!moveY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
    }
}
