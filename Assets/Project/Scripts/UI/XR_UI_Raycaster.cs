using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicXRCrossPlatformInput{

    public class XR_UI_Raycaster : MonoBehaviour
    {
        // Public Vars
        [Header("Raycast Settings")]
        public float RaycastDistance = Mathf.Infinity;
        public LayerMask UILayer;

        [Header("Input Settings")]
        public ButtonTypes UIInteractionButton = ButtonTypes.Trigger;

        [Header("Pointer")]
        public GameObject PointerPrefab;


        // Private Vars
        private GameObject pointer;
        private bool inputPressed = false;
        private bool inputUp = false;
        private Controller controller;
        private bool wasPointingAtLocation = false;
        private Transform hitElement;

        // Button related stuff

        private bool isButtonSelected = false;


        private void Start()
        {
            controller = GetComponent<Controller>();
            if(controller == null)
            {
                Debug.LogError("No controller script found please attatch one");
            }
            pointer = GameObject.Instantiate(PointerPrefab, this.transform);
            if (pointer == null)
            {
                Debug.LogError("No pointer prefab assigned on XR_UI_Raycaster please assign one to the gameobejct :" + this.transform.name);
            }
            else
            {
                pointer.transform.localPosition = Vector3.zero;
                pointer.transform.localRotation = Quaternion.identity;
                pointer.transform.localPosition = new Vector3(0, 0, pointer.transform.localScale.z / 2);
                pointer.SetActive(false);
            }
        }
        private void Update()
        {
            GetInput();
            CastRay();
        }


        private void GetInput()
        {
            inputPressed = XRCrossPlatformInputManager.Instance.GetInputByButton(UIInteractionButton,controller.Hand,true);
            inputUp = XRCrossPlatformInputManager.Instance.GetInputUp(UIInteractionButton, controller.Hand);
        }

        private void CastRay()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, RaycastDistance, UILayer))
            {
                // pointer stuff
                pointer.SetActive(true);
                pointer.transform.localScale = new Vector3(pointer.transform.localScale.x, pointer.transform.localScale.y, hit.distance);
                pointer.transform.localPosition = new Vector3(0, 0, pointer.transform.localScale.z / 2);

                hitElement = hit.transform;
                HandleButtonRaycast(hitElement);
                wasPointingAtLocation = true;
            }
            else
            {
                pointer.SetActive(false);
                TurnOffBeam();
                UnselectButton();
            }
        }

        /// <summary>
        /// Handles raycasting specifically buttons
        /// </summary>
        private void HandleButtonRaycast(Transform hitUI)
        {
            if (hitUI.GetComponent<Button>())
            {
                Button button = hitUI.GetComponent<Button>();

                if (inputPressed)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    isButtonSelected = false;
                }
                else if (!isButtonSelected)
                {
                    button.Select();
                    isButtonSelected = true;
                }

                if (inputUp)
                {
                    button.onClick.Invoke(); // Boom we clicked the button
                }
            }
        }
        private void TurnOffBeam()
        {
            if (wasPointingAtLocation)
            {
                wasPointingAtLocation = false;
            }
        }

        private void UnselectButton()
        {
            if (hitElement != null && isButtonSelected)
            {
                EventSystem.current.SetSelectedGameObject(null);
                isButtonSelected = false;
            }
        }

    }
}
