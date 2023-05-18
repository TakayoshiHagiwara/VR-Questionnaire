// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Input provider for debug. An empty Provider that does not perform any processing.
// -----------------------------------------------------------------------

using UnityEngine;

public class DebugInputProvider : IControllerInput
{
    Vector2 axis2d = new Vector2();

    public void Init() { }
    
    public Vector2 Axis2d()
    {
        return axis2d;
    }

    public bool IsPressSelectButton()
    {
        return false;
    }

    public bool IsLongPressSelectButton()
    {
        return false;
    }

    public bool IsPressNext()
    {
        return false;
    }

    public bool IsPressPrevious()
    {
        return false;
    }
}
