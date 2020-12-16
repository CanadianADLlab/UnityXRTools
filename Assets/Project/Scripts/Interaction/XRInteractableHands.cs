using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XRCrossPlatformInput;

namespace XRCrossPlatformInput
{
    /// <summary>
    /// Attactch with interactable objects to hide handss
    /// </summary>

    [RequireComponent(typeof(InteractableObject))]
    public class XRInteractableHands : MonoBehaviour
    {
        public GameObject LeftHandMesh;
        public GameObject RightHandMesh;

        public bool IsTwoHands = false;

        [DrawIf("isTwoHands", true)]  // the user doesn't need to see these if no hands
        public GameObject SecondaryLeftHandMesh;
        [DrawIf("isTwoHands", true)]
        public GameObject SecondaryRightHandMesh;


        private InteractableObject interactable;
        private InteractableSecondaryGrab secondaryInteractable; // not needed but some things might want to be two handed

        private bool isPrimaryHeld = false; // used to check if we need to turn on the secondary 

        // Start is called before the first frame update
        void Start()
        {
            interactable = GetComponent<InteractableObject>();
            secondaryInteractable = GetComponent<InteractableSecondaryGrab>();

            if (IsTwoHands && secondaryInteractable == null)
            {
                Debug.LogError("No Secondary grab attatched to an object with secondary grab hands on please attatch or disable on : " + this.transform);
            }
        }

        private void DisableMeshes()
        {
            if(LeftHandMesh == null || RightHandMesh == null)
            {
                Debug.LogError("No meshs to disabled on XRInteractHand on  : " + this.transform);
            }
            LeftHandMesh.SetActive(false);
            RightHandMesh.SetActive(false);
            if(IsTwoHands)
            {
                SecondaryLeftHandMesh.SetActive(false);
                SecondaryRightHandMesh.SetActive(false);
            }

        }
        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == interactable.ControllerLayer)
            {
                Controller xrGamepad = other.GetComponent<Controller>();
                if (!isPrimaryHeld)
                {
                    if (xrGamepad.Hand == ControllerHand.Left)
                    {
                        ToggleHand(LeftHandMesh, true);
                        xrGamepad.HideController();
                    }
                    else if (xrGamepad.Hand == ControllerHand.Right)
                    {
                        ToggleHand(RightHandMesh, true);
                        xrGamepad.HideController();
                    }
                }
                else if (IsTwoHands) // if already being primary held we check to make sure its a two hander before turning on the other hand
                {
                    if (xrGamepad.Hand == ControllerHand.Left)
                    {
                        ToggleHand(SecondaryLeftHandMesh, true);
                        xrGamepad.HideController();
                    }
                    else if (xrGamepad.Hand == ControllerHand.Right)
                    {
                        ToggleHand(SecondaryRightHandMesh, true);
                        xrGamepad.HideController();
                    }
                }
            }
        }

      

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == interactable.ControllerLayer)
            {
                Controller xrGamepad = other.GetComponent<Controller>();
                if (!isPrimaryHeld)
                {
                    if (xrGamepad.Hand == ControllerHand.Left)
                    {
                        ToggleHand(LeftHandMesh, false);
                        xrGamepad.ShowController();
                    }
                    else if (xrGamepad.Hand == ControllerHand.Right)
                    {
                        ToggleHand(RightHandMesh, false);
                        xrGamepad.ShowController();
                    }
                }
                else if (IsTwoHands) // if already being primary held we check to make sure its a two hander before turning on the other hand
                {
                    if (xrGamepad.Hand == ControllerHand.Left)
                    {
                        ToggleHand(SecondaryLeftHandMesh, false);
                        xrGamepad.ShowController();
                    }
                    else if (xrGamepad.Hand == ControllerHand.Right)
                    {
                        ToggleHand(SecondaryRightHandMesh, false);
                        xrGamepad.ShowController();
                    }
                }
            }
        }
        
        private void ToggleHand(GameObject hand, bool onOff)
        {
            hand.SetActive(onOff);
        }
    }
}



