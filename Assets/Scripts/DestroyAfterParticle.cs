using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<ParticleSystem>().isPlaying == true)
        {

            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
        }
    }
}
