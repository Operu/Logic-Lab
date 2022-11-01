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
            ImmediateReUpdate();
        }

        public void ImmediateReUpdate()
        {
            GetObjectsAtPosition(gridMousePos);
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
            return selectedObjects.Count > 0;
        }

        public List<WireInterface> GetObjectsAtPosition(Vector2 position)
        {
            selectedObjects.Clear();
            selectedWireInterfaces.Clear();
            
            Collider2D[] objectColliders = new Collider2D[2];
            int foundColliders = Physics2D.OverlapCircle(position, 0.1f, new ContactFilter2D().NoFilter(), objectColliders);
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
            return selectedWireInterfaces;
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