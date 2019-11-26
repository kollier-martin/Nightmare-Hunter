using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSwitch : MonoBehaviour
{
    public static bool KeyboardInput;
    public static bool JoyStickInput;

    public void Keyboard()
    {
        KeyboardInput = true;
        JoyStickInput = false;
    }

    public void Joystick()
    {
        JoyStickInput = true;
        KeyboardInput = false;
    }
}
