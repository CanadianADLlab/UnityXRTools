using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRCrossPlatformInput
{
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

        [Tooltip("Some guns are backwards enable this to make it correct")]
        public bool ReverseGrabLook = false;



        private InteractableObject mainGrab;
        private int controllerLayerMask;
        private Vector3 orginalGrabRotation = Vector3.zero;

        private Controller leftController;
        private Controller rightController;


        private bool rightButtonPressed = false;
        private bool leftButtonPressed = false;





        private void Start()
        {
            leftController = XRPositionManager.Instance.LeftHand.GetComponent<XRCrossPlatformInput.Controller>();
            rightController = XRPositionManager.Instance.RightHand.GetComponent<XRCrossPlatformInput.Controller>();
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
                if (ReverseGrabLook)
                {
                    transform.LookAt((2 * transform.position - rightController.transform.position)); // Look backwards
                }
                else
                {
                    transform.LookAt((rightController.transform.position));
                }

                transform.position = leftController.transform.position;
            }
            if (IsGrabbed && IsGrabbedLeft)
            {
                if (ReverseGrabLook)
                {
                    transform.LookAt((2 * transform.position - leftController.transform.position)); // Look backwards
                }
                else
                {
                    transform.LookAt((leftController.transform.position));
                }
                transform.position = rightController.transform.position;
            }


            if (!mainGrab.IsGrabbed && IsGrabbed)
            {
                ReleaseGrabRight();
                ReleaseGrabLeft();
            }

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
        private void UpdateController(Controller contr, bool updateValue)
        {
            contr.IsGrabbing = updateValue;
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
                                UpdateController(leftController, IsGrabbedLeft);
                                OnGrab(XRPositionManager.Instance.LeftHand.gameObject);
                            }
                        }

                        if (rightButtonPressed && other.tag.Equals("RightController") && !rightController.IsBeingUsed) // Making sure only the right can grab stuff if its the collision
                        {
                            if (!IsGrabbedRight)
                            {
                                rightController.IsBeingUsed = true;
                                IsGrabbedRight = true;
                                UpdateController(rightController, IsGrabbedRight);
                                OnGrab(XRPositionManager.Instance.RightHand.gameObject);
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
            OnGrab(XRPositionManager.Instance.LeftHand.gameObject);
        }



        private IEnumerator WaitToGrabRight()
        {
            yield return new WaitForEndOfFrame();
            print("Doing right grab");
            rightController.IsBeingUsed = true;
            IsGrabbedRight = true;
            OnGrab(XRPositionManager.Instance.RightHand.gameObject);
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
            mainGrab.ShowController(XRPositionManager.Instance.RightHand.gameObject);
        }

        private void ReleaseGrabLeft()
        {
            transform.localEulerAngles = orginalGrabRotation;
            IsGrabbedLeft = false;
            IsGrabbed = false;
            leftController.IsBeingUsed = false;
            leftController.IsGrabbing = false;
            mainGrab.ShowController(XRPositionManager.Instance.LeftHand.gameObject);
        }
    }
}