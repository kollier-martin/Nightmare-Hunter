using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cave : MonoBehaviour, IEventSystemHandler
{
    private AudioSource music;

    private void Awake()
    {
        music = GetComponent<AudioSource>();
    }
    public void TurnDownMusic()
    {
        music.volume = 0.5f;
    }

    public void TurnOffMusic()
    {
        music.Stop();
    }
}
