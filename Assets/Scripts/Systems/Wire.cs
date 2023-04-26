using System.Collections.Generic;
using System.Linq;
using System.Text;
using Components;
using Components.Types;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace Systems
{
    public class Wire : WireInterface
    {
        public List<SpriteRenderer> intersections;

        public List<Wire> connections;

        public Vector3 startPos;
        public Vector3 endPos;

        private bool active;
        private bool hasEvaluated;
        private LineRenderer wireLine;
        private EdgeCollider2D wireCollider;

        public override void ConnectWire(Wire wire)
        {
            if (connections.Contains(wire)) return;
            connections.Add(wire);
        }
        
        public void Initialize(Vector3 wireStartPos, Vector3 wireEndPos)
        {
            wireLine = GetComponent<LineRenderer>();
            wireCollider = GetComponent<EdgeCollider2D>();

            startPos = wireStartPos;
            endPos = wireEndPos;
            
            wireCollider.SetPoints(new List<Vector2> {startPos, endPos});

            Vector3 wireEdgeExtension = (endPos - startPos).normalized * 0.1f;
            wireLine.SetPositions(new Vector3[] { startPos - wireEdgeExtension, endPos + wireEdgeExtension});
            active = true;
        }

        public void Destroy()
        {
            active = false;
            RemoveConnections();
            Destroy(gameObject);
        }
        
        public void SoftDestroy()
        {
            transform.DestroyChildren();
            RemoveConnections();
        }

        public void Disable()
        {
            wireCollider.enabled = false;
        }
        
        public void RemoveConnections()
        {
            foreach (Wire connection in connections)
            {
                connection.connections.Remove(this);
            }
            connections.Clear();
        }

        public void ActivateState()
        {
            if (State) return;
            State = true;
            foreach (Wire wire in connections)
            {
                wire.ActivateState();
            }
            VisualUpdate();
        }

        public void ResetState() 
        {
            State = false;
            VisualUpdate();
        }

        private void VisualUpdate()
        {
            if (!active) return;
            Material material = State ? SimulationManager.Instance.wireOnMaterial : SimulationManager.Instance.wireOffMaterial;
            wireLine.material = material;
            foreach (SpriteRenderer intersection in intersections)
            {
                if (intersection != null)
                {
                    intersection.material = material;
                }
            }
        }
    }
}