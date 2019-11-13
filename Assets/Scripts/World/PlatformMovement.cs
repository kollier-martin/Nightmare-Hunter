using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    readonly float moveSpeed = 2.5f;
    bool moveY = true;
    bool moveX = true;

    // Update is called once per frame
    void Update()
    {
       if (name == "PX")
       {
            MoveX();
       }
       else if (name == "PY")
       {
            MoveY();
       }
    }

    void MoveX()
    {
        if (transform.position.x > 20f)
        {
            moveX = false;
        }

        if (transform.position.x < 10f)
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
        if (transform.position.y > 3f)
        {
            moveY = false;
        }

        if (transform.position.y < -1f)
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
