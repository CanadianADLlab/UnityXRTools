using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles getting the xr input
/// </summary>
public class XRCrossPlatformInputManager : MonoBehaviour
{
    // Devices
    private UnityEngine.XR.InputDevice leftHandDevice;
    private UnityEngine.XR.InputDevice rightHandDevice;
    private List<UnityEngine.XR.InputDevice> inputDevices;

    // bools for checking if input down
    private bool leftTriggerWasPressed = false;
    private bool rightTriggerWasPressed = false;

    private bool leftGripWasPressed = false;
    private bool rightGripWasPressed = false;

    // public bools

    [HideInInspector]
    public bool LeftTriggerPressed = false;
    [HideInInspector]
    public bool RightTriggerPressed = false;

    [HideInInspector]
    public bool LeftTriggerDown = false;
    [HideInInspector]
    public bool RightTriggerDown = false;

    [HideInInspector]
    public bool LeftGripPressed = false;
    [HideInInspector]
    public bool RightGripPressed = false;

    [HideInInspector]
    public bool LeftGripDown = false;
    [HideInInspector]
    public bool RightGripDown = false;

    // public vectors
    [HideInInspector]
    public Vector2 LeftStickAxis;
    [HideInInspector]
    public Vector2 RightStickAxis;


    void Start()
    {
        inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);


        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        leftHandDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand);
        rightHandDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        GetTriggerInput();
        GetGripInput();
        GetJoyStickAxisInput();
    }


    private void GetTriggerInput()
    {
        leftTriggerWasPressed = LeftTriggerPressed; // Check before input so its the value last frame
        rightTriggerWasPressed = RightTriggerPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out LeftTriggerPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out RightTriggerPressed);
       
        if(LeftTriggerPressed)
        {
            LeftTriggerDown = false;
        }
        if(LeftTriggerPressed && !leftTriggerWasPressed)
        {
            LeftTriggerDown = true;
        }

        if (RightTriggerPressed)
        {
            RightTriggerDown = false;
        }
        if (RightTriggerPressed && !rightTriggerWasPressed)
        {
            RightTriggerDown = true;
        }
    }


    private void GetGripInput()
    {
        leftGripWasPressed = LeftGripPressed; // Check before input so its the value last frame
        rightGripWasPressed = RightGripPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out LeftGripPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out RightGripPressed);

        if (LeftGripPressed)
        {
            LeftGripDown = false;
        }
        if (LeftGripPressed && !leftGripWasPressed)
        {
            LeftGripDown = true;
        }

        if (RightGripPressed)
        {
            RightGripDown = false;
        }
        if (RightGripPressed && !rightGripWasPressed)
        {
            RightGripDown = true;
        }
    }

    private void GetJoyStickAxisInput()
    {
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out LeftStickAxis);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out RightStickAxis);

        print("Left hand axis " + LeftStickAxis);
        print("Right hand axis " + RightStickAxis);
    }


    #region singleton

    public static XRCrossPlatformInputManager Instance;
    private void Awake()
    {        
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("XRCrossPlatformInputManager already exist destorying one attatched to " + this.transform);
            Destroy(Instance);
        }
    }

    #endregion
}
