// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Input provider for OVR controller (Meta quest, Rift...).
// -----------------------------------------------------------------------

using UnityEngine;

public class OVRControllerInputProvider : IControllerInput
{
    OVRInput.Controller targetHand = OVRInput.Controller.RTouch;

    float deltaTime;
    const float longPressTime = 1f;

    public void Init()
    {
        deltaTime = 0;
    }

    public Vector2 Axis2d()
    {
        return OVRInput.Get(OVRInput.RawAxis2D.Any, targetHand);
    }

    public bool IsPressSelectButton()
    {
        return OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight);
    }

    public bool IsLongPressSelectButton()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
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
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, targetHand);
    }

    public bool IsPressPrevious()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, targetHand);
    }
}
