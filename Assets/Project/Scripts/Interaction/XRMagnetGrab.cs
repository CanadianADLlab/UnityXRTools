using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicXRCrossPlatformInput
{
    /// <summary>
    /// This script attatches to the controller and handles grabbing things from distance
    /// </summary>
    public class XRMagnetGrab : MonoBehaviour
    {
        [Header("Image")]
        public Sprite GrabIcon; // Little icon that displays on item when raycasted at

        [Header("Sphere Cast Settings")]

        public float GrabDistance = 10; // How far the controller can grab from in mtrs
        public LayerMask GrabLayer;
        public Vector3 SphereCastDirection = new Vector3(0,0,1);
        public float SphereCastRadius = 0.35f;


        private Controller xrGamePad;


        private void Start()
        {
            xrGamePad = GetComponent<Controller>();
        }

        private void DrawDebugRay()
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(transform.TransformDirection(SphereCastDirection)));
        }
        private void Update()
        {
            CastRayFromController();
            DrawDebugRay();
        }
        private void CastRayFromController()
        {
            if(!xrGamePad.IsGrabbing)
            {
                RaycastHit hit;
                if(Physics.SphereCast(transform.position,SphereCastRadius,transform.TransformDirection(SphereCastDirection),out hit,GrabDistance,GrabLayer))
                {
                    print("we hit something before the if statement : " + hit.transform.name);
                    if (hit.collider.transform.GetComponent<InteractableObject>()) // we hit a grabable boy
                    {
                        print("We hit something that has interactable : " + hit.transform.name);
                    }
                }
            }
        }

    }
}
