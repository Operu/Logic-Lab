using UnityEngine;

namespace User
{
    public class PlayerCamera : MonoBehaviour
    {

        public float minZoom;
        public float maxZoom;
        public float zoomAmount;
    
    
    
        private Camera mainCam;
    
    
        private void Start()
        {
            mainCam = Camera.main;
        }

        void Update()
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize + zoomAmount, minZoom, maxZoom);
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize - zoomAmount, minZoom, maxZoom);
            }
        }
    }
}
