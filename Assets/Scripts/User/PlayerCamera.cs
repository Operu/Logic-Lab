using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace User
{
    public class PlayerCamera : MonoBehaviour
    {

        public float moveSpeed;
        private Vector3 movement;
        
        public float minZoom;
        public float maxZoom;
        public float zoomAmount;

        [SerializeField] private Transform background;
        public float updateCooldown;
        public bool isBackgroundMovementActive;

        private Vector3 lastPos;

        private Camera mainCam;
    
    
        private void Start()
        {
            mainCam = Camera.main;
            StartCoroutine(UpdateBackgroundPosition());
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
                Vector3 roundedPos = Utility.RoundVector2(mainCam.transform.position);
                if (roundedPos != lastPos)
                {
                    lastPos = roundedPos;
                    background.position = roundedPos + Vector3.forward;
                }
            }
        }

        public void MoveInput(InputAction.CallbackContext context)
        {
            movement = (Vector3)context.ReadValue<Vector2>() * moveSpeed;
        }
    }
}
