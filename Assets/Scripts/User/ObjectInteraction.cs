using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using UnityEngine;

namespace User
{
    public class ObjectInteraction : MonoBehaviour
    {
        [SerializeField] private List<GameObject> selectedObjects;
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
            UpdateSelectedObjects();
            if (selectedObjects.Count > 0)
            {
                EnableCursor(gridMousePos);
            }
            else
            {
                DisableCursor();
            }
        }

        public bool IsHoveringObject()
        {
            return selectedObjects.ToList().Count > 0;
        }

        private void UpdateSelectedObjects()
        {
            selectedObjects.Clear();
            selectedWireInterfaces.Clear();
            
            Collider2D[] objectColliders = new Collider2D[2];
            int foundColliders = Physics2D.OverlapCircle(gridMousePos, 0.1f, new ContactFilter2D().NoFilter(), objectColliders);
            if (foundColliders > 0)
            {
                foreach (Collider2D objectCollider in objectColliders)
                {
                    if (!objectCollider) continue;


                    selectedObjects.Add(objectCollider.gameObject);
                    if (objectCollider.TryGetComponent(out WireInterface wireInterface))
                    {
                        selectedWireInterfaces.Add(wireInterface);
                    }
                }
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