using System.Collections.Generic;
using System.Linq;
using Managers;
using Systems;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

namespace User
{
    public class WiringTool : MonoBehaviour
    {
        [SerializeField] private Transform wireStorage;
        [SerializeField] private GameObject wirePrefab;
        
        [SerializeField] private LineRenderer previewWireToCorner;
        [SerializeField] private LineRenderer previewWireToPos;

        [SerializeField] private ObjectInteraction interaction;
        
        private bool isPlacingWire;

        private bool isCornerOnXAxis;

        private Vector3 wireOriginPos;
        private Vector3 wireCornerRelativePos;
        private Vector3 wireDestinationRelativePos;
        private Vector2 gridMousePos;
        
        // Left mouse button event 
        public void InteractInput(InputAction.CallbackContext context)
        {
            if (context.started) StartWirePreview();
            if (context.canceled) StopWirePreview();
        }
        
        // Mouse movement event
        public void UpdateMousePosEvent(Vector2 newGridMousePos)
        {
            gridMousePos = newGridMousePos;
            if (isPlacingWire) UpdateWirePreview();
        }

        private void StartWirePreview()
        {
            if (interaction.IsHoveringObject())
            {
                isPlacingWire = true;
                wireOriginPos = gridMousePos;

                previewWireToCorner.gameObject.SetActive(true);
                previewWireToPos.gameObject.SetActive(true);
            }
        }
        
        private void StopWirePreview()
        {
            isPlacingWire = false;

            GameObject originWire = Instantiate(wirePrefab, wireStorage);

            var positions = new Vector3[2];
            previewWireToCorner.GetPositions(positions);
            originWire.GetComponent<LineRenderer>().SetPositions(positions);
            originWire.GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2>() 
                { wireOriginPos, wireOriginPos + wireCornerRelativePos});
            
            if (previewWireToPos.GetPosition(0) != previewWireToPos.GetPosition(1))
            {
                GameObject cornerWire = Instantiate(wirePrefab, wireStorage);
                positions = new Vector3[2];
                previewWireToPos.GetPositions(positions);
                cornerWire.GetComponent<LineRenderer>().SetPositions(positions);
                cornerWire.GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2>()
                    { wireOriginPos + wireCornerRelativePos, gridMousePos });
            }
            
            previewWireToCorner.SetPositions(new [] { Vector3.zero, Vector3.zero });
            previewWireToPos.SetPositions(new [] { Vector3.zero, Vector3.zero });

            previewWireToCorner.gameObject.SetActive(false);
            previewWireToPos.gameObject.SetActive(false); 
        }

        private void CalculateCornerPos()
        {
            if (isCornerOnXAxis && wireDestinationRelativePos.x == 0)
            {
                wireCornerRelativePos = wireDestinationRelativePos;
                isCornerOnXAxis = false;
                return;
            }

            if (!isCornerOnXAxis && wireDestinationRelativePos.y == 0)
            {
                wireCornerRelativePos = wireDestinationRelativePos;
                isCornerOnXAxis = true;
                return;
            }

            wireCornerRelativePos = isCornerOnXAxis
                ? new Vector3(wireDestinationRelativePos.x, 0)
                : new Vector3(0, wireDestinationRelativePos.y);
        }
        

        private void UpdateWirePreview()
        {
            wireDestinationRelativePos = (Vector3)gridMousePos - wireOriginPos;
            CalculateCornerPos();

            Vector3 wireCornerPos = wireOriginPos + wireCornerRelativePos;

            Vector3 cornerOffset = (wireCornerPos - wireOriginPos).normalized * 0.1f;
            Vector3 posOffset = ((Vector3)gridMousePos - wireCornerPos).normalized * 0.1f;
            Debug.Log(cornerOffset);

            previewWireToCorner.SetPositions(new [] { wireOriginPos - cornerOffset, wireCornerPos + cornerOffset});
            previewWireToPos.SetPositions(new [] { wireCornerPos - posOffset, (Vector3)gridMousePos + posOffset});
        }
    }
}