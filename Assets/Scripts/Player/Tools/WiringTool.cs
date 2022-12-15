using System.Collections.Generic;
using System.Linq;
using Managers;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Tools
{
    public class WiringTool : MonoBehaviour
    {
        [SerializeField] private Transform wireStorage;
        [SerializeField] private GameObject wirePrefab;
        [SerializeField] private GameObject intersectionPrefab;
        
        [SerializeField] private LineRenderer previewWireToCorner;
        [SerializeField] private LineRenderer previewWireToPos;

        [SerializeField] private PlayerInteraction interaction;

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
                List<Wire> firstWireSet = TryCreateWires(wireOriginPos, wireCornerPos);
                foreach (Wire wire in firstWireSet)
                {
                    AddWireConnections(wire);
                    SimulationManager.Instance.AddWireToSimulation(wire);
                }
            }

            if (previewWireToPos.GetPosition(0) != previewWireToPos.GetPosition(1))
            {
                List<Wire> secondWireSet = TryCreateWires(wireCornerPos, gridMousePos);
                foreach (Wire wire in secondWireSet)
                {
                    AddWireConnections(wire);
                    SimulationManager.Instance.AddWireToSimulation(wire);
                }
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

        private List<Wire> TryCreateWires(Vector3 startPos, Vector3 endPos)
        {
            List<Wire> createdWires = new();

            List<bool> wiredOccupiedList = new();

            Vector3 stepPos = startPos;
            Vector3 lineIntervalStep = (endPos - startPos).normalized * 0.5f;

            while (stepPos != endPos)
            {
                List<Wire> leftEdgeWires = interaction.GetWiresOnPosition(stepPos);
                List<Wire> rightEdgeWires = interaction.GetWiresOnPosition(stepPos + lineIntervalStep);

                bool found = false;
                foreach (Wire leftWirePart in leftEdgeWires)
                {
                    if (rightEdgeWires.Contains(leftWirePart)) found = true;
                }
                wiredOccupiedList.Add(found);
                stepPos += lineIntervalStep;
            }

            int index = 0;
            bool isCreatingWire = false;
            Vector3 currentStart = startPos;
            
            foreach (bool isOccupied in wiredOccupiedList)
            {
                if (!isOccupied && !isCreatingWire)
                {
                    isCreatingWire = true;
                    currentStart = lineIntervalStep * index + startPos;
                    Debug.Log(currentStart);
                }

                if (isCreatingWire && isOccupied)
                {
                    createdWires.Add(CreateWire(currentStart, lineIntervalStep * index + startPos));
                    isCreatingWire = false;
                }   
                index++;
            }

            if (isCreatingWire && !wiredOccupiedList.Last())
            {
                createdWires.Add(CreateWire(currentStart, endPos));
            }
            
            return createdWires;
        }
        
        private Wire CreateWire(Vector3 startPos, Vector3 endPos)
        {
            Wire wire = Instantiate(wirePrefab, wireStorage).GetComponent<Wire>();
            wire.Initialize(startPos, endPos);
            return wire;
        }

        public void AddWireConnections(Wire wire)
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
                TryPlaceIntersection(stepPos, wire, newWires);
                stepPos += lineIntervalStep;
            }
            
        }

        private void TryPlaceIntersection(Vector3 position, Wire originWire, List<Wire> otherWires)
        {
            if (ShouldPlaceIntersection(position, originWire, otherWires))
            {
                GameObject intersection = Instantiate(intersectionPrefab, position, Quaternion.identity, originWire.transform);
                originWire.intersections.Add(intersection.GetComponent<SpriteRenderer>());
            }
        }
        
        private bool ShouldPlaceIntersection(Vector3 position, Wire originWire, List<Wire> otherWires)
        {
            if (otherWires.Count < 1) return false;
            if (otherWires.Count > 1) return true;

            // Is the subject wire connected to the other wires at the stepPos?
            foreach (Wire newWire in otherWires)
            {
                if (!originWire.connections.Contains(newWire)) return false;
            }

            if (originWire.startPos != position && originWire.endPos != position)
            {
                return true;
            }

            if (otherWires[0].startPos != position && otherWires[0].endPos != position)
            {
                return true;
            }

            return false;
        }
    }
}