using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chest : MonoBehaviour, IEventSystemHandler
{
    public bool PlayerOn = false;
    private int i;
    public bool open = false;

    private System.Random rand;
    private Animator anim;
    private SpriteRenderer mySR;
    private AudioSource Sound;

    public List<GameObject> Item;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        anim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();
        Sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Open") && !open && PlayerOn == true)
        {
            open = true;
            Open();
        }
    }

    public void Open()
    {
        anim.SetBool("Open", true);
        Sound.Play();

        if (Item.Count == 1)
        {
            ExecuteEvents.Execute<Player>(FindObjectOfType<Player>().gameObject, null, (x, y) => x.GetItem(Item[0]));
            Item.Remove(Item[0]);
        }
        else if (Item.Count > 1)
        {
            i = rand.Next(Item.Count + 1);
            ExecuteEvents.Execute<Player>(FindObjectOfType<Player>().gameObject, null, (x, y) => x.GetItem(Item[i]));
            Item.Remove(Item[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case ("Player"):
                PlayerOn = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case ("Player"):
                PlayerOn = false;
                break;
        }
    }
}
