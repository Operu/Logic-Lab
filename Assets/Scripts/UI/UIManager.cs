using System;
using Managers;
using Player;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject placementBackground;
        private Transform placementBackgroundGraphics;

        [SerializeField] private PlayerSelection selection;
        [SerializeField] private Transform componentStorage;
        
        private bool isPlacing;
        private GameObject currentlyPlacing;
        private Vector3 mousePos = Vector3.zero;

        private void Start()
        {
            placementBackgroundGraphics = placementBackground.transform.GetChild(0);
        }

        public void PlaceComponentInput(InputAction.CallbackContext context)
        {
            if (context.started && isPlacing && currentlyPlacing)
            {
                isPlacing = false;
                placementBackground.SetActive(false);
                SimulationManager.Instance.TryAddToSimulation(currentlyPlacing.transform);
                
                selection.active = true;
            }
        }

        public void RotateComponentInput(InputAction.CallbackContext context)
        {
            if (context.started && isPlacing && currentlyPlacing)
            {
                currentlyPlacing.transform.localRotation ;
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

        public void SelectComponent(PlacementItem item)
        {
            if (!isPlacing)
            {
                selection.active = false;
                
                isPlacing = true;
                placementBackground.transform.position = mousePos;
                placementBackgroundGraphics.localPosition = item.offset;
                
                placementBackgroundGraphics.localScale = item.size;
                placementBackground.SetActive(true);
                currentlyPlacing = Instantiate(item.prefab, mousePos, quaternion.identity, componentStorage);
            }
        }
    }
}