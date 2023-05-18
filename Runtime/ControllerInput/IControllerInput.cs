// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Interface for controller input.
// -----------------------------------------------------------------------

using UnityEngine;

public interface IControllerInput
{
    // Initialize
    void Init();

    // 2D axis value such as touch pad, stick...
    Vector2 Axis2d();

    // Press select button. Process called only once.
    bool IsPressSelectButton();

    // Long press select button. Process called while pressing.
    bool IsLongPressSelectButton();

    // Press next button
    bool IsPressNext();

    // Press previous button
    bool IsPressPrevious();
}
