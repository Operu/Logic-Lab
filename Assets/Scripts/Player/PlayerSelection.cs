using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Player
{
    public class PlayerSelection : MonoBehaviour
    {
        public bool active = true;
        
        public List<GameObject> selectedObjects;
        public List<WireInterface> selectedWireInterfaces;

        private GameObject selectionCursor;
        private SpriteRenderer selectionCursorRenderer;
        
        private Vector3 gridMousePos;
        private bool cursorHold;

        private void Start()
        {   
            selectionCursor = transform.GetChild(0).gameObject;
            selectionCursorRenderer = selectionCursor.GetComponent<SpriteRenderer>();
        }


        public void UpdateMousePos(Vector2 newGridMousePos)
        {
            gridMousePos = newGridMousePos;
            if (active) ImmediateReUpdate();
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
            
            Collider2D[] objectColliders = new Collider2D[3];
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

        public List<Wire> GetWiresOnEdge(Vector2 aPos, Vector2 bPos)
        {
            Vector2 realPos = Vector2.Lerp(aPos, bPos, 0.5f);
            return GetWiresOnPosition(realPos);
        }
        
        public List<Wire> GetWiresOnPosition(Vector2 position)
        {
            List<WireInterface> connections = GetObjectsAtPosition(position);
            List<Wire> returnWires = new();
            foreach (WireInterface connection in connections)
            {
                if (connection as Wire == null) continue;
                returnWires.Add(connection as Wire);
            }
            return returnWires;
        }

        public GameObject GetComponentOnMouse()
        {
            Collider2D[] objectColliders = new Collider2D[1];
            ContactFilter2D filter = new();
            filter.layerMask = LayerMask.GetMask("Component");
            int foundColliders = Physics2D.OverlapCircle(gridMousePos, 0.1f, filter, objectColliders);
            if (foundColliders > 0)
            {
                return objectColliders[0].gameObject;
            }
            return null;
        }

        private void EnableCursor(Vector3 location)
        {
            selectionCursor.SetActive(true);
            selectionCursor.transform.position = location;
        }

        private void DisableCursor()
        {
            selectionCursor.SetActive(false);
        }

        public void CursorHoldToggle()
        {
            cursorHold = !cursorHold;
            selectionCursorRenderer.color = cursorHold ? Color.yellow : Color.white;
        }
    }
}