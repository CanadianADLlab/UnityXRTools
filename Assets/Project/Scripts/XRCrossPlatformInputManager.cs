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

    // public bools

    public bool LeftTriggerPressed = false;
    public bool RightTriggerPressed = false;


    public bool LeftTriggerDown = false;
    public bool RightTriggerDown = false;


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
        GetTriggerInputs();
    }


    private void GetTriggerInputs()
    {
        print("Here left " + LeftTriggerPressed);
        print("Here right " + RightTriggerPressed);
        print("Here left down " + LeftTriggerDown);
        print("Here right down " + RightTriggerDown);
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
