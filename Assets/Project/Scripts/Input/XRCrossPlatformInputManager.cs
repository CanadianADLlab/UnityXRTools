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


    private bool leftMenuButtonWasPressed = false;
    private bool rightMenuButtonWasPressed = false;

    private bool leftStickWasPressed = false;
    private bool rightStickWasPressed = false;

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

    [HideInInspector]
    public bool LeftMenuButtonDown = false;
    [HideInInspector]
    public bool RightMenuButtonDown = false;

    [HideInInspector]
    public bool LeftMenuButtonPressed = false;
    [HideInInspector]
    public bool RightMenuButtonPressed = false;

    [HideInInspector]
    public bool LeftStickDown = false;
    [HideInInspector]
    public bool RightStickDown = false;

    [HideInInspector]
    public bool LeftStickPressed = false;
    [HideInInspector]
    public bool RightStickPressed = false;

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
        GetMenuButtonInputs();
        GetStickPressedInputs();
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


    /// <summary>
    /// Grabs menu button inputs, right menu button on the oculus is the actual home menu and doesn't work so we should never use that
    /// </summary>
    private void GetMenuButtonInputs()
    {
        leftMenuButtonWasPressed = LeftMenuButtonPressed; // Check before input so its the value last frame
        rightMenuButtonWasPressed = RightMenuButtonPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out LeftMenuButtonPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out RightMenuButtonPressed);

        if (LeftMenuButtonPressed)
        {
            LeftMenuButtonDown = false;
        }
        if (LeftMenuButtonPressed && !leftMenuButtonWasPressed)
        {
            LeftMenuButtonDown = true;
        }

        if (RightMenuButtonPressed)
        {
            RightMenuButtonDown = false;
        }
        if (RightMenuButtonPressed && !rightMenuButtonWasPressed)
        {
            RightMenuButtonDown = true;
        }
    }

    /// <summary>
    /// Stick inputs (Maybe shouldn't be called stick because on some devices it might not be)
    /// </summary>
    private void GetStickPressedInputs()
    {
        leftStickWasPressed = LeftStickPressed; // Check before input so its the value last frame
        rightStickWasPressed = RightStickPressed; // Check before input so its the value last frame
        leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out LeftStickPressed);
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out RightStickPressed);
        
        if (LeftStickPressed)
        {
            LeftStickDown = false;
        }
        if (LeftStickPressed && !leftStickWasPressed)
        {
            LeftStickDown = true;
        }

        if (RightStickPressed)
        {
            RightStickDown = false;
        }
        if (RightStickPressed && !rightStickWasPressed)
        {
            RightStickDown = true;
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
/// <summary>
/// Handles all the input 
/// </summary>
/// 
public enum ButtonTypes
{
    Grip,
    Primary,
    Secondary,
    Trigger
}

