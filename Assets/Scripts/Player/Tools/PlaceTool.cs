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
        [SerializeField] private PlayerManager player;
        [SerializeField] private Transform placementBackground;

        [SerializeField] private SelectTool selectTool;
        [SerializeField] private Transform componentStorage;

        private PlacementItem currentlyPlacingItem;
        private GameObject currentlyPlacing;

        public void PlaceComponent()
        {
            if (currentlyPlacing)
            {
                placementBackground.gameObject.SetActive(false);
                SimulationManager.Instance.TryAddToSimulation(currentlyPlacing.transform);

                BaseComponent newPlacement = currentlyPlacing.GetComponent<BaseComponent>();
                newPlacement.Initialize();
                // TODO: Ryk ansvar til simulationManager

                foreach (Pin pin in newPlacement.IO)
                {
                    List<Wire> wires = selectTool.GetWiresOnPosition(pin.transform.position);
                    foreach (Wire wire in wires)
                    {
                        pin.ConnectWire(wire);
                    }
                }
            }
        }

        public void SelectComponent(PlacementItem item)
        {
            currentlyPlacingItem = item;
            placementBackground.position = player.MousePosition + (Vector3)item.offset;
            placementBackground.localScale = item.size;
            
            placementBackground.gameObject.SetActive(true);
            currentlyPlacing = Instantiate(item.prefab, player.MousePosition, quaternion.identity, componentStorage);
        }
        
        public void RotateComponent()
        {
            // TODO: Create rotation feature
        }
        
        public void UpdatePlacingPosition()
        {
            currentlyPlacing.transform.position = player.MousePosition;
            placementBackground.transform.position = player.MousePosition + (Vector3)currentlyPlacingItem.offset;
        }
        
    }
}