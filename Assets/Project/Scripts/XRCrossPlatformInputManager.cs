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

    private bool leftPrimaryButtonWasPressed = false;
    private bool rightPrimaryButtonWasPressed = false;

    private bool leftSecondaryButtonWasPressed = false;
    private bool rightSecondaryButtonWasPressed = false;

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


    [HideInInspector]
    public bool LeftPrimaryButtonPressed = false;
    [HideInInspector]
    public bool RightPrimaryButtonPressed = false;

    [HideInInspector]
    public bool LeftSecondaryButtonPressed = false;
    [HideInInspector]
    public bool RightSecondaryButtonPressed = false;

    [HideInInspector]
    public bool LeftPrimaryButtonDown = false;
    [HideInInspector]
    public bool RightPrimaryButtonDown = false;

    [HideInInspector]
    public bool LeftSecondaryButtonDown = false;
    [HideInInspector]
    public bool RightSecondaryButtonDown = false;

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
        GetPrimaryButtonInput();
        GetSecondaryButtonInput();
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
    }

    /// <summary>
    /// Gets the input of what ever the primary button is on each controller
    /// </summary>
    private void GetPrimaryButtonInput()
    {
        leftPrimaryButtonWasPressed = LeftPrimaryButtonPressed; // Check before input so its the value last frame
        rightPrimaryButtonWasPressed = RightPrimaryButtonPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out LeftPrimaryButtonPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out RightPrimaryButtonPressed);
        print("Left Primary pressed " + LeftPrimaryButtonPressed);
        print("Right Primary pressed " + RightPrimaryButtonPressed);
        print("Left Primary down " + LeftPrimaryButtonDown);
        print("Right Primary down " + RightPrimaryButtonDown);

        if (LeftPrimaryButtonPressed)
        {
            LeftPrimaryButtonDown = false;
        }
        if (LeftPrimaryButtonPressed && !leftPrimaryButtonWasPressed)
        {
            LeftPrimaryButtonDown = true;
        }

        if (RightPrimaryButtonPressed)
        {
            RightPrimaryButtonDown = false;
        }
        if (RightPrimaryButtonPressed && !rightPrimaryButtonWasPressed)
        {
            RightPrimaryButtonDown = true;
        }
    }

    /// <summary>
    /// Gets the input of what ever the secondary button is on each controller
    /// </summary>
    private void GetSecondaryButtonInput()
    {
        leftSecondaryButtonWasPressed = LeftSecondaryButtonPressed; // Check before input so its the value last frame
        rightSecondaryButtonWasPressed = RightSecondaryButtonPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out LeftSecondaryButtonPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out RightSecondaryButtonPressed);

        print("Left Secondary pressed " + LeftSecondaryButtonPressed);
        print("Right Secondary pressed " + RightSecondaryButtonPressed);
        print("Left Secondary down " + LeftSecondaryButtonDown);
        print("Right Secondary down " + RightSecondaryButtonDown);
        if (LeftSecondaryButtonPressed)
        {
            LeftSecondaryButtonDown = false;
        }
        if (LeftSecondaryButtonPressed && !leftSecondaryButtonWasPressed)
        {
            LeftSecondaryButtonDown = true;
        }

        if (RightSecondaryButtonPressed)
        {
            RightSecondaryButtonDown = false;
        }
        if (RightSecondaryButtonPressed && !rightSecondaryButtonWasPressed)
        {
            RightSecondaryButtonDown = true;
        }
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
