using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player.Systems
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed;

        [Header("Camera Zoom")]
        public float minZoom;
        public float maxZoom;
        public float zoomAmount;

        [Header("Background")]
        [SerializeField] private Transform background;
        public float updateCooldown;
        public bool isBackgroundMovementActive;

        
        private Vector3 movement;
        private Vector3 lastBackgroundPos;

        private Camera mainCam;
    
    
        private void Start()
        {
            mainCam = Camera.main;
            StartCoroutine(UpdateBackgroundPosition());
        }
        
        public void MoveInput(InputAction.CallbackContext context)
        {
            movement = (Vector3)context.ReadValue<Vector2>() * moveSpeed;
        }

        private void Update()
        {
            transform.position += movement * Time.deltaTime;
            if (Input.mouseScrollDelta.y < 0)
            {
                mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize + zoomAmount, minZoom, maxZoom);
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize - zoomAmount, minZoom, maxZoom);
            }
        }

        private IEnumerator UpdateBackgroundPosition()
        { 
            while (isBackgroundMovementActive)
            {
                yield return new WaitForSeconds(updateCooldown);
                Vector3 roundedPos = Helpers.RoundVector2(mainCam.transform.position);
                if (roundedPos != lastBackgroundPos)
                {
                    lastBackgroundPos = roundedPos;
                    background.position = roundedPos + Vector3.forward;
                }
            }
        }
    }
}
