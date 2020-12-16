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

        public bool isTwoHands = false;

        [DrawIf("isTwoHands", true)]  // the user doesn't need to see these if no hands
        public GameObject SecondaryLeftHandMesh;
        [DrawIf("isTwoHands", true)] 
        public GameObject SecondaryRightHandMesh;




        private InteractableObject interactable;
        private InteractableSecondaryGrab secondaryInteractable; // not needed but some things might want to be two handed

        // Start is called before the first frame update
        void Start()
        {
            interactable = GetComponent<InteractableObject>();
            secondaryInteractable = GetComponent<InteractableSecondaryGrab>();
          
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ShowHands() // Checks to see whats grabbing and from there enables
        {

        }
    }
}



