using System;
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
        [Header("Image Prefab")]
        public GameObject GrabIconPrefab; // Little icon that displays on item when raycasted at

        [Header("Sphere Cast Settings")]

        public float GrabDistance = 10; // How far the controller can grab from in mtrs
        public LayerMask GrabLayer;
        public Vector3 SphereCastDirection = new Vector3(0,0,1);
        public float SphereCastRadius = 0.15f;


        private Controller xrGamePad;

        private GameObject handSprite;
        private InteractableObject objectToBeGrabbed;

        private void Start()
        {
            xrGamePad = GetComponent<Controller>();
            if (GrabIconPrefab != null)
            {
                handSprite = GameObject.Instantiate(GrabIconPrefab);
                ToggleHandIcon(false);
            }
          
        }

        private void DrawDebugRay()
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(transform.TransformDirection(SphereCastDirection)));
        }
        private void Update()
        {
            CastRayFromController();
            DrawDebugRay();
            CheckIfGrabbed();
        }

        private void CheckIfGrabbed()
        {
            if(objectToBeGrabbed != null && objectToBeGrabbed.IsGrabbed)
            {
                ToggleHandIcon(false);
            }
        }

        private void CastRayFromController()
        {
            if(!xrGamePad.IsGrabbing)
            {
                RaycastHit hit;
                if(Physics.SphereCast(transform.position,SphereCastRadius,transform.TransformDirection(SphereCastDirection),out hit,GrabDistance,GrabLayer))
                {
                    // could be the object, could be a parent, could be a child who knows lets be certain though
                    if (hit.collider.transform.GetComponent<InteractableObject>() != null)
                    {
                        objectToBeGrabbed = hit.collider.transform.GetComponent<InteractableObject>();
                    }
                    else if(hit.collider.transform.GetComponentInChildren<InteractableObject>() != null)
                    {
                        objectToBeGrabbed = hit.collider.transform.GetComponentInChildren<InteractableObject>();
                    }
                    else if(hit.collider.transform.GetComponentInParent<InteractableObject>() != null)
                    {
                        objectToBeGrabbed = hit.collider.transform.GetComponentInParent<InteractableObject>();
                    }

                    if (objectToBeGrabbed != null && objectToBeGrabbed.SnapToController ) // we hit a grabable boy and we need to make sure it has snap mode 
                    {
                        ToggleHandIcon(true);
                        objectToBeGrabbed.CheckGrab(GetComponent<Collider>()); // This function handles grabbing, uses the controller collider to attatch
                      
                        if (!objectToBeGrabbed.HoldToGrab && objectToBeGrabbed.IsGrabbed) // if in the above function the user grabs the thing we need to turn off the icon
                        {
                            ToggleHandIcon(false);
                        }
                        handSprite.transform.position = hit.transform.position;
                    }
                    else
                    {
                        ToggleHandIcon(false);
                    }
                }
                else
                {
                    ToggleHandIcon(false);
                }
            }
        }

       
        private void ToggleHandIcon(bool onOff)
        {
            if (handSprite != null)
            {
                handSprite.SetActive(onOff);
            }
        }

    }

}
