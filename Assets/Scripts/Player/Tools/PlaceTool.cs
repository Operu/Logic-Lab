using System.Collections.Generic;
using Components.Types;
using Managers;
using Systems;
using UI;
using Unity.Mathematics;
using UnityEngine;

namespace Player.Tools
{
    public class PlaceTool : MonoBehaviour
    {
        [SerializeField] private Transform placementBackground;

        [SerializeField] private SelectTool selection;
        [SerializeField] private Transform componentStorage;
        
        private bool isPlacing;
        private GameObject currentlyPlacing;
        private Vector3 mousePos = Vector3.zero;

        public void PlaceComponent()
        {
            if (isPlacing && currentlyPlacing)
            {
                isPlacing = false;
                placementBackground.gameObject.SetActive(false);
                SimulationManager.Instance.TryAddToSimulation(currentlyPlacing.transform);

                BaseComponent newPlacement = currentlyPlacing.GetComponent<BaseComponent>();
                newPlacement.Initialize();

                foreach (Pin pin in newPlacement.IO)
                {
                    List<Wire> wires = selection.GetWiresOnPosition(pin.transform.position);
                    foreach (Wire wire in wires)
                    {
                        pin.ConnectWire(wire);
                    }
                }
                
                selection.active = true;
            }
        }

        public void SelectComponent(PlacementItem item)
        {
            if (!isPlacing)
            {
                isPlacing = true;
                placementBackground.position = mousePos;
                placementBackground.localPosition = item.offset;
                
                placementBackground.localScale = item.size;
                placementBackground.gameObject.SetActive(true);
                currentlyPlacing = Instantiate(item.prefab, mousePos, quaternion.identity, componentStorage);
            }
        }
        
        public void RotateComponent()
        {
            if (isPlacing)
            {
                // TODO: Create rotation feature
            }
        }
        
        public void UpdatePlacingPosition(Vector2 newGridMousePos)
        {
            mousePos = newGridMousePos;
            if (isPlacing)
            {
                currentlyPlacing.transform.position = mousePos;
                placementBackground.transform.position = mousePos;
            }
        }
        
    }
}