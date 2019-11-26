using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool Keyboard, Controller = false;
    public Chest TutorialChest;
    public GameObject[] KeyboardPopUps;
    public GameObject[] ControllerPopUps;

    private int ControllerPopUpIndex, KeyBoardPopUpIndex = 0;
    int JumpCounter = 0;

    private void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard == true)
        {
            for (int i = 0; i < KeyboardPopUps.Length; i++)
            {
                if (i == KeyBoardPopUpIndex)
                {
                    KeyboardPopUps[i].SetActive(true);
                }
                else
                {
                    KeyboardPopUps[i].SetActive(false);
                }
            }
            switch (KeyBoardPopUpIndex)
            {
                case 0:
                    // Movement
                    if (Input.GetAxisRaw("Horizontal") > 0.5 || Input.GetAxisRaw("Horizontal") < -0.5)
                    {
                        KeyBoardPopUpIndex++;
                    }
                    break;
                case 1:
                    // Jump
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        KeyBoardPopUpIndex++;
                    }
                    break;
                case 2:
                    // Double Jumps
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        JumpCounter++;
                        if (JumpCounter == 2)
                        {
                            KeyBoardPopUpIndex++;
                        }
                    }
                    break;
                case 3:
                    // Chest Open
                    if (TutorialChest.open == true)
                    {
                        KeyBoardPopUpIndex++;
                    }
                    break;
                case 4:
                    // Weapon Switch
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    {
                        KeyBoardPopUpIndex++;
                    }
                    break;
                case 5:
                    // Shoot Enemy
                    if (Input.GetButtonDown("Fire1"))
                    {
                        KeyBoardPopUpIndex++;
                    }
                    break;
            }
        }
        else if (Controller == true)
        {
            Time.timeScale = 1;

            for (int i = 0; i < ControllerPopUps.Length; i++)
            {
                if (i == ControllerPopUpIndex)
                {
                    ControllerPopUps[i].SetActive(true);
                }
                else
                {
                    ControllerPopUps[i].SetActive(false);
                }
            }
            switch (ControllerPopUpIndex)
            {
                case 0:
                    // Movement
                    if (Input.GetAxisRaw("Horizontal") > 0.5 || Input.GetAxisRaw("Horizontal") < -0.5)
                    {
                        ControllerPopUpIndex++;
                    }
                    break;
                case 1:
                    // Jump
                    if (Input.GetButtonDown("Jump"))
                    {
                        ControllerPopUpIndex++;
                    }
                    break;
                case 2:
                    // Double Jumps
                    if (Input.GetButtonDown("Jump"))
                    {
                        JumpCounter++;
                        if (JumpCounter == 2)
                        {
                            ControllerPopUpIndex++;
                        }
                    }
                    break;
                case 3:
                    // Chest Open
                    if (TutorialChest.open == true)
                    {
                        ControllerPopUpIndex++;
                    }
                    break;
                case 4:
                    // Weapon Switch
                    if (Input.GetAxis("DPadX") == 1 || Input.GetAxis("DPadX") == -1)
                    {
                        ControllerPopUpIndex++;
                    }
                    break;
                case 5:
                    // Shoot Enemy
                    if (Input.GetAxis("Fire1") > 0)
                    {
                        ControllerPopUpIndex++;
                    }
                    break;
            }
        }
    }

    public void SetController()
    {
        Time.timeScale = 1;
        Controller = true;

        InputSwitch.JoyStickInput = true;
    }

    public void SetKeyboard()
    {
        Time.timeScale = 1;
        Keyboard = true;

        InputSwitch.KeyboardInput = true;
    }
}
