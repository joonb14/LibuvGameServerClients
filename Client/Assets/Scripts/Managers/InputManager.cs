using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    public Action KeyAction = null;
    public bool _keyPressed = false;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
        {
            _keyPressed = false;
            return;
        }
        if (KeyAction != null)
        {
            _keyPressed = true;
            KeyAction.Invoke();
        }
    }
}
