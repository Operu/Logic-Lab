using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Components.Types;
using Managers;
using Systems;
using UnityEngine;

public class WiringTool : MonoBehaviour
{
    public Transform wireStorage;
    
    public GameObject wirePrefab;
    
    public LineRenderer previewWire;
    public GameObject previewWireStub;
    public GameObject selectionCursor;

    private List<GameObject> hoveredObjects;
    private bool isPlacingWire;
    private Vector2 gridMousePos;

    private List<GameObject> wireStartObjects;
    private List<GameObject> wireEndObjects;
    
    private List<Vector3> wirePositionSequence;

    // Start is called before the first frame update
    private void Start()
    {
        hoveredObjects = new List<GameObject>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        gridMousePos = Utility.GridMousePos();
        
        HandleObjectSelection();
        if (Input.GetMouseButtonDown(0) && hoveredObjects.Any())
        {
            StartWirePlacement();
            isPlacingWire = true;
            return;
        }

        if (Input.GetMouseButtonUp(0) && isPlacingWire)
        {
            StopWirePlacement();
            isPlacingWire = false;
        }

        if (isPlacingWire)
        {
            UpdatePreviewWirePlacement();
        }
    }

    private void StartWirePlacement()
    {
        wireStartObjects = hoveredObjects;
        wirePositionSequence = new List<Vector3>();
        
        previewWireStub.SetActive(true);
        previewWireStub.transform.position = gridMousePos;
        
        previewWire.gameObject.SetActive(true);
        previewWire.SetPosition(0, gridMousePos);
        wirePositionSequence.Add(gridMousePos);
    }

    private void StopWirePlacement()
    {
        if (previewWire.positionCount > 1)
        {
            wireEndObjects = hoveredObjects;
            
            GameObject newWire = Instantiate(wirePrefab, wireStorage);
            Wire wire = newWire.GetComponent<Wire>();
            wire.Initialize(wirePositionSequence);
            
            AddWireToSimulation(wire);
        }
        previewWire.gameObject.SetActive(false);
        previewWire.positionCount = 1;
        wirePositionSequence.Clear();
        
        previewWireStub.SetActive(false);
        previewWireStub.transform.position = Vector3.zero;
        

        wireEndObjects.Clear();
        wireStartObjects.Clear();
    }

    private void UpdatePreviewWirePlacement()
    {
        if ((Vector2)previewWire.GetPosition(previewWire.positionCount - 1) != gridMousePos)
        {
            previewWireStub.transform.position = gridMousePos;
            if (previewWire.positionCount > 1)
            {
                if ((Vector2) previewWire.GetPosition(previewWire.positionCount - 2) == gridMousePos)
                {
                    previewWire.positionCount--;
                    wirePositionSequence.RemoveAt(wirePositionSequence.Count - 1);
                    return;
                }
            }
            previewWire.positionCount++;
            int currentIndex = previewWire.positionCount - 1;

            previewWire.SetPosition(currentIndex, gridMousePos);
            wirePositionSequence.Add(gridMousePos);
        }
    }


    private void HandleObjectSelection()
    {
        Collider2D[] objectColliders = new Collider2D[2];
        int foundColliders = Physics2D.OverlapCircle(gridMousePos, 0.1f, new ContactFilter2D().NoFilter(), objectColliders);
        if (foundColliders > 0)
        {
            List<GameObject> newHoveredObjects = new List<GameObject>();
            foreach (Collider2D objectCollider in objectColliders)
            {
                if (!objectCollider) continue;
                GameObject colliderGameObject = objectCollider.gameObject;
                newHoveredObjects.Add(colliderGameObject);
            }
            hoveredObjects = newHoveredObjects;
            selectionCursor.SetActive(true);
            selectionCursor.transform.position = gridMousePos;
        }
        else
        {
            selectionCursor.SetActive(false);
            hoveredObjects.Clear();
        }
    }

    private void AddWireToSimulation(Wire wire)
    {
        foreach (GameObject wireInput in wireStartObjects)
        {
            wireInput.GetComponent<WireInterface>().ConnectWire(wire);
        }

        foreach (GameObject inputPin in wireEndObjects)
        {
            if (inputPin.CompareTag("Pin"))
            {
                inputPin.GetComponent<Pin>().ConnectWire(wire);
            }
        }
        SimulationManager.Instance.AddWireToSimulation(wire);
    }
}