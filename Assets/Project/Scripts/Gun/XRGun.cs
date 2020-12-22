using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EpicXRCrossPlatformInput
{
    /// <summary>
    /// Probably don't need an XR gun... but we need a gun
    /// </summary>
    [RequireComponent(typeof(InteractableObject))]
    public class XRGun : MonoBehaviour
    {
        public ButtonTypes ShootButton = ButtonTypes.Trigger;

        public bool SingleFire = true;


        private InteractableObject interactableObject;
        private bool leftShoot = false;
        private bool rightShoot = false;


        private void Start()
        {
            interactableObject = GetComponent<InteractableObject>();
        }

        private void UpdateGrabButton()
        {
            if (!SingleFire)
            {
                if (ShootButton == ButtonTypes.Grip)
                {
                    leftShoot = XRCrossPlatformInputManager.Instance.LeftGripPressed;
                    rightShoot = XRCrossPlatformInputManager.Instance.RightGripPressed;
                }
                else if (ShootButton == ButtonTypes.Trigger)
                {
                    leftShoot = XRCrossPlatformInputManager.Instance.LeftTriggerPressed;
                    rightShoot = XRCrossPlatformInputManager.Instance.RightTriggerPressed;
                }
            }
            else
            {
                if (ShootButton == ButtonTypes.Grip)
                {
                    leftShoot = XRCrossPlatformInputManager.Instance.LeftGripDown;
                    rightShoot = XRCrossPlatformInputManager.Instance.RightGripDown;
                }
                else if (ShootButton == ButtonTypes.Trigger)
                {
                    leftShoot = XRCrossPlatformInputManager.Instance.LeftTriggerDown;
                    rightShoot = XRCrossPlatformInputManager.Instance.RightTriggerDown;
                }
            }

        }
    }

}