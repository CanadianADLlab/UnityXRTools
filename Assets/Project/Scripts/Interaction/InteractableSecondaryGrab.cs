﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement of rotation 
/// </summary>

[RequireComponent(typeof(InteractableObject))]
public class InteractableSecondaryGrab : MonoBehaviour
{
    [HideInInspector]
    public bool IsGrabbedLeft = false;
    [HideInInspector]
    public bool IsGrabbedRight = false;
    [HideInInspector]
    public bool IsGrabbed = false;

  

    private InteractableObject mainGrab;
    private int controllerLayerMask;
    private Vector3 orginalGrabRotation = Vector3.zero;

    private XRController leftController;
    private XRController rightController;


    private bool rightButtonPressed = false;
    private bool leftButtonPressed = false;



    private void Start()
    {
        leftController = XRPositionManager.Instance.LeftHand.GetComponent<XRController>();
        rightController = XRPositionManager.Instance.RightHand.GetComponent<XRController>();
        mainGrab = GetComponent<InteractableObject>();

        controllerLayerMask = mainGrab.ControllerLayer;
    }


    private void UpdateGrabButton()
    {
        if (mainGrab.HoldToGrab)
        {
            if (mainGrab.GrabButton == ButtonTypes.Grip)
            {
                leftButtonPressed = XRCrossPlatformInputManager.Instance.LeftGripPressed;
                rightButtonPressed = XRCrossPlatformInputManager.Instance.RightGripPressed;
            }
            else if (mainGrab.GrabButton == ButtonTypes.Trigger)
            {
                leftButtonPressed = XRCrossPlatformInputManager.Instance.LeftTriggerPressed;
                rightButtonPressed = XRCrossPlatformInputManager.Instance.RightTriggerPressed;
            }
        }
        else
        {
            if (mainGrab.GrabButton == ButtonTypes.Grip)
            {
                leftButtonPressed = XRCrossPlatformInputManager.Instance.LeftGripDown;
                rightButtonPressed = XRCrossPlatformInputManager.Instance.RightGripDown;
            }
            else if (mainGrab.GrabButton == ButtonTypes.Trigger)
            {
                leftButtonPressed = XRCrossPlatformInputManager.Instance.LeftTriggerDown;
                rightButtonPressed = XRCrossPlatformInputManager.Instance.RightTriggerDown;
            }
        }

    }

    private void Update()
    {
        UpdateGrabButton(); // Just checks for the input of the grab button

        if (IsGrabbed && IsGrabbedRight)
        {
            // Maybe add an offset to move down
            // transform.LookAt((2 * transform.position - rightController.transform.position)); // Look backwards
            transform.LookAt((rightController.transform.position));
            transform.position = leftController.transform.position;
        }
        if (IsGrabbed && IsGrabbedLeft)
        {
            //  transform.LookAt((2 * transform.position  -leftController.transform.position)); // Look backwards
            transform.LookAt((leftController.transform.position));
            transform.position = rightController.transform.position;
        }


        if (!mainGrab.IsGrabbed && IsGrabbed)
        {
            ReleaseGrabRight();
            ReleaseGrabLeft();
        }

        UpdateControllers();
    }

    private void CheckRelease()
    {
        if (!leftButtonPressed && IsGrabbedLeft && IsGrabbed)
        {
            ReleaseGrabLeft();
        }

        if (!rightButtonPressed && IsGrabbedRight && IsGrabbed)
        {
            ReleaseGrabRight();
        }
    }

    /// <summary>
    /// Handles updating the controllers on whats going on
    /// </summary>
    private void UpdateControllers()
    {
        leftController.IsGrabbing = IsGrabbedLeft;
        rightController.IsGrabbing = IsGrabbedRight;
    }
    private void DrawDebugRay()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == controllerLayerMask)
        {
            print("Is this being triggered");
            if (mainGrab.IsGrabbed) // this can only work if a primary grab has been set
            {
                print("How deep");
                if (mainGrab.HoldToGrab)
                {
                    print("are we this deep");
                    print("left pressed " + leftButtonPressed);
                    print("tag  " + other.tag);
                    print("left being used " + leftController.IsBeingUsed);
                    if (leftButtonPressed && other.tag.Equals("LeftController") && !leftController.IsBeingUsed) // Making sure only the left can grab stuff if its the collision
                    {
                        print("maybe this deep?");
                        if (!IsGrabbedLeft)
                        {
                            print("maybe even this far my guy"); 
                            leftController.IsBeingUsed = true;
                            IsGrabbedLeft = true;
                            OnGrab(XRPositionManager.Instance.LeftHand);
                        }
                    }

                    if (rightButtonPressed && other.tag.Equals("RightController") && !rightController.IsBeingUsed) // Making sure only the right can grab stuff if its the collision
                    {
                        if (!IsGrabbedRight)
                        {
                            rightController.IsBeingUsed = true;
                            IsGrabbedRight = true;
                            OnGrab(XRPositionManager.Instance.RightHand);
                        }
                    }
                }
                else
                {
                    if (leftButtonPressed && other.tag.Equals("LeftController") && !leftController.IsBeingUsed) // Making sure only the left can grab stuff if its the collision
                    {
                        if (!IsGrabbedLeft)
                        {
                            StartCoroutine(WaitToGrabLeft());
                        }
                    }

                    if (leftButtonPressed && other.tag.Equals("RightController") && !rightController.IsBeingUsed) // Making sure only the right can grab stuff if its the collision
                    {
                        if (!IsGrabbedRight)
                        {
                            StartCoroutine(WaitToGrabRight());
                        }
                    }
                }
            }
            CheckRelease();
        }
    }


    private IEnumerator WaitToGrabLeft()
    {
        yield return new WaitForEndOfFrame();
        print("Doing left grab");
        leftController.IsBeingUsed = true;
        IsGrabbedLeft = true;
        OnGrab(XRPositionManager.Instance.LeftHand);
    }



    private IEnumerator WaitToGrabRight()
    {
        yield return new WaitForEndOfFrame();
        print("Doing right grab");
        rightController.IsBeingUsed = true;
        IsGrabbedRight = true;
        OnGrab(XRPositionManager.Instance.RightHand);
    }


    /// <summary>
    /// Called when we grab the object
    /// </summary>
    private void OnGrab(GameObject controller)
    {
        IsGrabbed = true;
        orginalGrabRotation = transform.localEulerAngles;

        mainGrab.HideController(controller);
    }

    private void ReleaseGrabRight()
    {
        transform.localEulerAngles = orginalGrabRotation;
        IsGrabbedRight = false;
        IsGrabbed = false;
        rightController.IsBeingUsed = false;
        rightController.IsGrabbing = false;
        mainGrab.ShowController(XRPositionManager.Instance.RightHand);
    }

    private void ReleaseGrabLeft()
    {
        transform.localEulerAngles = orginalGrabRotation;
        IsGrabbedLeft = false;
        IsGrabbed = false;
        leftController.IsBeingUsed = false;
        leftController.IsGrabbing = false;
        mainGrab.ShowController(XRPositionManager.Instance.LeftHand);
    }
}