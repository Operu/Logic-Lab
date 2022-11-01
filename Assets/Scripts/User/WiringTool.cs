using System.Collections.Generic;
using System.Linq;
using Managers;
using Systems;
using UnityEditor.VersionControl;
using UnityEngine;
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
            if (!isPlacingWire) return;
            isPlacingWire = false;

            Wire firstWire = null;
            Wire secondWire = null;
            
            if (previewWireToCorner.GetPosition(0) != previewWireToCorner.GetPosition(1))
            {
                firstWire = Instantiate(wirePrefab, wireStorage).GetComponent<Wire>();
                var positions = new List<Vector2>
                {
                    [0] = previewWireToCorner.GetPosition(0),
                    [1] = previewWireToCorner.GetPosition(1)
                };
                firstWire.Initialize(positions);
            }

            if (previewWireToPos.GetPosition(0) != previewWireToPos.GetPosition(1))
            {
                secondWire = Instantiate(wirePrefab, wireStorage).GetComponent<Wire>();
                var positions = new List<Vector2>
                {
                    [0] = previewWireToPos.GetPosition(0),
                    [1] = previewWireToPos.GetPosition(1)
                };
                secondWire.Initialize(positions);
            }
            
            if (firstWire != null) AddWireConnections(firstWire);
            if (secondWire != null) AddWireConnections(secondWire);
             
            previewWireToCorner.SetPositions(new [] { Vector3.zero, Vector3.zero });
            previewWireToPos.SetPositions(new [] { Vector3.zero, Vector3.zero });

            previewWireToCorner.gameObject.SetActive(false);
            previewWireToPos.gameObject.SetActive(false); 
            
            interaction.ImmediateReUpdate();
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

        private void AddWireConnections(Wire wire)
        {
            Vector3 stepPos = wire.startPos;
            Vector3 lineIntervalStep = (wire.endPos - wire.startPos).normalized * 0.5f;
            while (stepPos != wire.endPos)
            {
                List<WireInterface> connections = interaction.GetObjectsAtPosition(stepPos);
                foreach (WireInterface connection in connections)
                {
                    Wire newWire = connection as Wire;
                    if (newWire != null)
                    {
                        wire.ConnectWire(newWire);
                        newWire.ConnectWire(wire);
                    }
                }
                stepPos += lineIntervalStep;
            }
            
        }
    }
}