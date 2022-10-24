using System.Collections.Generic;
using System.Linq;
using Managers;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace User
{
    public class WiringTool : MonoBehaviour
    {
        [SerializeField] private Transform wireStorage;
        [SerializeField] private GameObject wirePrefab;
        
        [SerializeField] private LineRenderer previewWire;
        [SerializeField] private GameObject previewWireStub;

        [SerializeField] private GameObject wirePartPrefab;

        [SerializeField] private ObjectInteraction interaction;
        
        private bool isPlacingWire;
        private Vector2 gridMousePos;
        
        private Vector3 wireOriginPos;
        
        private List<GameObject> wireOriginObjects;
        private List<GameObject> wireEndObjects;

        private List<Vector3> wirePath;

        // Start is called before the first frame update
        private void Start()
        {
            wirePath = new List<Vector3>();
            wireOriginObjects = new List<GameObject>();
            wireEndObjects = new List<GameObject>();
        }

        // Left mouse button event 
        public void InteractInput(InputAction.CallbackContext context)
        {
            if (context.started) StartWirePreviewPlacer();
            if (context.canceled) StopWirePreview();
        }
        
        public void UpdateMousePosEvent(Vector2 newGridMousePos)
        {
            if (isPlacingWire)
            {
                Vector3 posChange = newGridMousePos - gridMousePos;
                if (wirePath.Count > 0)
                {
                    if (wirePath.Last() + posChange != Vector3.zero)
                    {
                        wirePath.Add(posChange);
                    }
                    else
                    {
                        wirePath.Remove(wirePath.Last());
                    }
                }
            }
            gridMousePos = newGridMousePos;
            if (isPlacingWire) UpdateWirePreview();
        }

        private void StartWirePreviewPlacer()
        {
            if (interaction.selectedWireInterfaces.Count ! > 0) return;
            
            isPlacingWire = true;
            wireOriginPos = gridMousePos;
            
            wireOriginObjects = interaction.selectedObjects;
            

            previewWireStub.SetActive(true);
            previewWireStub.transform.position = gridMousePos;
        
            previewWire.gameObject.SetActive(true);
            previewWire.SetPosition(0, wireOriginPos);
        }
        
        private void StopWirePreview()
        {
            isPlacingWire = false;
            if (wirePath.Count > 0)
            {
                wireEndObjects = interaction.selectedObjects;
            }
            ResetWirePreview();
        }
        

        private void UpdateWirePreview()
        {
            previewWireStub.transform.position = gridMousePos;

            previewWire.positionCount = 1;
            Vector3 summedPosition = wireOriginPos;
            
            foreach (Vector3 direction in wirePath)
            {
                previewWire.positionCount++;
                summedPosition += direction;
                previewWire.SetPosition(previewWire.positionCount, summedPosition);
            }
        }
        
        private void ResetWirePreview()
        {
            previewWire.gameObject.SetActive(false);
            previewWire.positionCount = 1;
            wirePath.Clear();
        
            previewWireStub.SetActive(false);
            previewWireStub.transform.position = Vector3.zero;
        

            wireEndObjects.Clear();
            wireOriginObjects.Clear();
        }

        private void CreateWire()
        {
            
        }

        private void CreateNewWirePart(Vector3 origin, Vector3 destination)
        {
            WirePart wirePart = Instantiate(wirePartPrefab).GetComponent<WirePart>();
            wirePart.PositionLine(wireOriginPos, destination);
        }
    }
}