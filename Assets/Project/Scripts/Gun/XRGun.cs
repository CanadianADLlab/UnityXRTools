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
        public bool Automatic = true;

        public float Clipsize = 12;



        private InteractableObject interactableObject;
        private bool leftShoot = false;
        private bool rightShoot = false;


        private void Start()
        {
            interactableObject = GetComponent<InteractableObject>();
        }

        private void Update()
        {
            UpdateGrabButton();
        }
        private void UpdateGrabButton()
        {
            leftShoot = XRCrossPlatformInputManager.Instance.GetInputByButton(ShootButton, ControllerHand.Left, Automatic);
            rightShoot = XRCrossPlatformInputManager.Instance.GetInputByButton(ShootButton, ControllerHand.Right, Automatic);
            print("Left shoot is " + leftShoot);
            print("Left shoot is " + rightShoot);
        }
    }

}