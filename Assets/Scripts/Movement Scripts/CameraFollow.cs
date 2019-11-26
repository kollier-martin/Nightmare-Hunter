using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour, IEventSystemHandler
{
    private Vector2 velocity;
    public Animator camAnim;

    public GameObject player;

    public float smoothTimeX;
    public float smoothTimeY;

    public bool bounds;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {
            // Locks camera bounds to specified values
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                                             Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                                             Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }

    public void Shake()
    {
        camAnim.SetTrigger("Shake");
    }
}
