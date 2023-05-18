// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Input provider for keyboard. For debug.
// -----------------------------------------------------------------------

using UnityEngine;

public class KeyboardInputProvider : IControllerInput
{
    float x, y;
    Vector2 axis2d = new Vector2();

    float deltaTime;
    const float longPressTime = 1f;

    public void Init()
    {
        deltaTime = 0;
    }

    public Vector2 Axis2d()
    {
        x = 0;
        y = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            x = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            x = -1;

        if (Input.GetKey(KeyCode.UpArrow))
            y = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            y = -1;

        axis2d.x = x;
        axis2d.y = y;

        return axis2d;
    }

    public bool IsPressSelectButton()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            return true;

        return false;
    }

    public bool IsLongPressSelectButton()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > longPressTime)
                return true;
        }
        else
            deltaTime = 0;

        return false;
    }

    public bool IsPressNext()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            return true;

        return false;
    }

    public bool IsPressPrevious()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            return true;

        return false;
    }
}
