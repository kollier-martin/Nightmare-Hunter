using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer = null;

    // Start is called before the first frame update
    public void Reset()
    {
        mixer.SetFloat("Master", 1);
        mixer.SetFloat("Music", 1);
        mixer.SetFloat("FX", 1);
    }
}
