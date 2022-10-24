﻿using System;
using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace User
{
    public class ObjectInteraction : MonoBehaviour
    {
        public List<GameObject> selectedObjects;
        public List<WireInterface> selectedWireInterfaces;

        private GameObject selectionCursor;
        private Vector2 gridMousePos;

        private void Start()
        {   
            selectionCursor = transform.GetChild(0).gameObject;
        }


        public void UpdateMousePos(Vector2 newGridMousePos)
        {
            gridMousePos = newGridMousePos;
            UpdateObjectSelection();
        }

        private void UpdateObjectSelection()
        {
            Collider2D[] objectColliders = new Collider2D[2];
            int foundColliders = Physics2D.OverlapCircle(gridMousePos, 0.1f, new ContactFilter2D().NoFilter(), objectColliders);
            if (foundColliders > 0)
            {
                selectedObjects.Clear();
                selectedWireInterfaces.Clear();
                foreach (Collider2D objectCollider in objectColliders)
                {
                    if (!objectCollider) continue;
                    selectedObjects.Add(objectCollider.gameObject);
                    if (objectCollider.TryGetComponent(out WireInterface wireInterface))
                    {
                        selectedWireInterfaces.Add(wireInterface);
                    }
                }
                EnableCursor(gridMousePos);
            }
            else
            {
                DisableCursor();
            }
        }

        private void EnableCursor(Vector3 location)
        {
            selectionCursor.SetActive(true);
            selectionCursor.transform.position = gridMousePos;
        }

        private void DisableCursor()
        {
            selectionCursor.SetActive(false);
        }
    }
}