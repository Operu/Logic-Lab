using System.Collections.Generic;
using Managers;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace User.Tools
{
    public class WiringTool : MonoBehaviour
    {
        [SerializeField] private Transform wireStorage;
        [SerializeField] private GameObject wirePrefab;
        
        [SerializeField] private LineRenderer previewWireToCorner;
        [SerializeField] private LineRenderer previewWireToPos;

        [SerializeField] private ObjectInteraction interaction;

        private List<Wire> hoveredWires;
        
        private bool isPlacingWire;
        private bool isCornerOnXAxis;

        private Vector3 wireOriginPos;
        private Vector3 wireCornerPos;
        private Vector3 gridMousePos;
        
        // Left mouse button event 
        public void InteractInput(InputAction.CallbackContext context)
        {
            if (context.started && interaction.IsHoveringObject()) StartWirePreview();
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
            isPlacingWire = true;
            wireOriginPos = gridMousePos;

            previewWireToCorner.gameObject.SetActive(true);
            previewWireToPos.gameObject.SetActive(true);
        }
        
        private void StopWirePreview()
        {
            if (!isPlacingWire) return;
            isPlacingWire = false;

            if (previewWireToCorner.GetPosition(0) != previewWireToCorner.GetPosition(1))
            {
                Wire firstWire = CreateWire(wireOriginPos, wireCornerPos);
                AddWireConnections(firstWire);
                SimulationManager.Instance.AddWireToSimulation(firstWire);
            }

            if (previewWireToPos.GetPosition(0) != previewWireToPos.GetPosition(1))
            {
                Wire secondWire = CreateWire(wireCornerPos, gridMousePos);
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
                List<Wire> newWires = new();
                foreach (WireInterface connection in connections)
                {
                    Wire wireConnection = connection as Wire;
                    if (wireConnection != null && wireConnection != wire)
                    {
                        // If the wire found at every step exists, and its not equal to the placed wire.
                        newWires.Add(wireConnection);
                        if (wireConnection.startPos == stepPos || wireConnection.endPos == stepPos || wire.startPos == stepPos || wire.endPos == stepPos)
                        {
                            wire.ConnectWire(wireConnection);
                            wireConnection.ConnectWire(wire);
                        }
                    }

                    Pin newPin = connection as Pin;
                    if (newPin != null)
                    {
                        newPin.ConnectWire(wire);
                    }
                }

                if (ShouldPlaceIntersection(stepPos, wire, newWires))
                {
                    GameObject intersection = Instantiate(Manager.Instance.intersectionPrefab, stepPos, Quaternion.identity, wire.transform);
                    wire.intersection = intersection.GetComponent<SpriteRenderer>();
                }

                stepPos += lineIntervalStep;
            }
            
        }

        private bool ShouldPlaceIntersection(Vector3 position, Wire originWire, List<Wire> otherWires)
        {
            if (otherWires.Count < 1) return false;
            foreach (Wire newWire in otherWires)
            {
                if (newWire.transform.childCount > 0) return false;
                if (!originWire.connections.Contains(newWire)) return false;
            }
            
            if (otherWires.Count > 1)
            {
                return true;
            }

            if (otherWires[0].startPos != position && otherWires[0].endPos != position)
            {
                return true;
            }

            return false;
        }

        /*private bool IsPlacingInsideSameWire()
        {
            List<Wire> newHoveredWires = new List<Wire>();
            List<WireInterface> newHoveredInterfaces = interaction.GetObjectsAtPosition(gridMousePos);
            foreach (WireInterface wireInterface in newHoveredInterfaces)
            {
                Wire wire = wireInterface as Wire;
                if (wire != null) newHoveredWires.Add(wire);
            }
            
            if (newHoveredWires.Any(wire => hoveredWires.Contains(wire)))
            {
                return true;
            }
            hoveredWires = newHoveredWires;
            return false;

        }*/
    }
}