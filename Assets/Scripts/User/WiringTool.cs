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
        private Vector3 wireCornerPos;
        private Vector3 gridMousePos;
        
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
                firstWire = CreateWire(wireOriginPos, wireCornerPos);
            }

            if (previewWireToPos.GetPosition(0) != previewWireToPos.GetPosition(1))
            {
                secondWire = CreateWire(wireCornerPos, gridMousePos);
            }

            if (firstWire != null)
            {
                AddWireConnections(firstWire);
                SimulationManager.Instance.AddWireToSimulation(firstWire);
            }

            if (secondWire != null)
            {
                AddWireConnections(secondWire);
                SimulationManager.Instance.AddWireToSimulation(secondWire);
            }
            
             
            previewWireToCorner.SetPositions(new [] { Vector3.zero, Vector3.zero });
            previewWireToPos.SetPositions(new [] { Vector3.zero, Vector3.zero });

            previewWireToCorner.gameObject.SetActive(false);
            previewWireToPos.gameObject.SetActive(false); 
            
            interaction.ImmediateReUpdate();
        }

        private void UpdateWirePreview()
        {
            CalculateCornerPos();

            Vector3 cornerWireExtension = (wireCornerPos - wireOriginPos).normalized * 0.1f;
            Vector3 positionWireExtension = (gridMousePos - wireCornerPos).normalized * 0.1f;

            previewWireToCorner.SetPositions(new [] { wireOriginPos - cornerWireExtension, wireCornerPos + cornerWireExtension});
            previewWireToPos.SetPositions(new [] { wireCornerPos - positionWireExtension, gridMousePos + positionWireExtension});
        }
        
        
        
        private void CalculateCornerPos()
        {
            Vector3 wireDestinationRelativePos = gridMousePos - wireOriginPos;
            
            if (isCornerOnXAxis && wireDestinationRelativePos.x == 0)
            {
                wireCornerPos = gridMousePos;
                isCornerOnXAxis = false;
                return;
            }

            if (!isCornerOnXAxis && wireDestinationRelativePos.y == 0)
            {
                wireCornerPos = gridMousePos;
                isCornerOnXAxis = true;
                return;
            }

            wireCornerPos = isCornerOnXAxis
                ? new Vector3(wireDestinationRelativePos.x, 0) + wireOriginPos
                : new Vector3(0, wireDestinationRelativePos.y) + wireOriginPos;
        }

        private Wire CreateWire(Vector3 startPos, Vector3 endPos)
        {
            Wire wire = Instantiate(wirePrefab, wireStorage).GetComponent<Wire>();
            wire.Initialize(startPos, endPos);
            return wire;
        }

        private void AddWireConnections(Wire wire)
        {
            Vector3 stepPos = wire.startPos;
            Vector3 lineIntervalStep = (wire.endPos - wire.startPos).normalized * 0.5f;
            while (stepPos != wire.endPos + lineIntervalStep)
            {
                List<WireInterface> connections = interaction.GetObjectsAtPosition(stepPos);
                foreach (WireInterface connection in connections)
                {
                    Wire newWire = connection as Wire;
                    if (newWire != null && newWire != wire)
                    {
                        if (newWire.startPos == stepPos || newWire.endPos == stepPos)
                        {
                            wire.ConnectWire(newWire);
                            newWire.ConnectWire(wire);
                            if (!(wire.endPos == newWire.startPos || wire.endPos == newWire.startPos))
                            {
                                GameObject intersection = Instantiate(Manager.Instance.intersectionPrefab, stepPos, Quaternion.identity, wire.transform);
                                wire.intersection = intersection.GetComponent<SpriteRenderer>();
                            }
                        }
                    }

                    Pin newPin = connection as Pin;
                    if (newPin != null)
                    {
                        newPin.ConnectWire(wire);
                    }
                }
                stepPos += lineIntervalStep;
            }
            
        }
    }
}