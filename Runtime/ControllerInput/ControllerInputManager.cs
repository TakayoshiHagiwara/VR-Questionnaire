// -----------------------------------------------------------------------
// Author:  Takayoshi Hagiwara (Toyohashi University of Technology)
// Created: 2020/11/22
// Summary: Manager for controller input.
// -----------------------------------------------------------------------

using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{
    public IControllerInput controllerInput;

    public enum InputMode
    {
        DEBUG = 0,
        KEYBOARD = 1,
        OVR_CONTROLLER = 2,
    }
    [SerializeField, Header("Input mode")]
    private InputMode inputMode;


    // Start is called before the first frame update
    void Start()
    {
        ChangeProvider(inputMode);
    }

    /// <summary>
    /// Change input provider
    /// </summary>
    /// <param name="mode">Select the input mode</param>
    public void ChangeProvider(InputMode mode)
    {
        switch (mode)
        {
            case InputMode.DEBUG:
                controllerInput = new DebugInputProvider();
                break;
            case InputMode.KEYBOARD:
                controllerInput = new KeyboardInputProvider();
                break;
            case InputMode.OVR_CONTROLLER:
                controllerInput = new OVRControllerInputProvider();
                break;
        }

        controllerInput.Init();
    }
}
