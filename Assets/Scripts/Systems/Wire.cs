using System.Collections.Generic;
using System.Text;
using Components;
using Components.Types;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems
{
    public class Wire : WireInterface
    {
        public GameObject intersection;

        public List<Wire> connections;

        public Vector3 startPos;
        public Vector3 endPos;

        private bool hasEvaluated;
        private LineRenderer wireRenderer;
        private EdgeCollider2D wireCollider;

        public override void ConnectWire(Wire wire)
        {
            connections.Add(wire);
        }
        
        public void Initialize(List<Vector2> wirePoints)
        {
            wireRenderer = GetComponent<LineRenderer>();
            wireCollider = GetComponent<EdgeCollider2D>();

            startPos = wirePoints[0];
            endPos = wirePoints[1];
            
            wireCollider.SetPoints(wirePoints);

            Vector3 wireDirection = (endPos - startPos).normalized * 0.1f;
            wireRenderer.SetPositions(new Vector3[] { startPos - wireDirection, endPos + wireDirection});
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
            Material material = State ? Manager.Instance.wireOn : Manager.Instance.wireOff;
            wireRenderer.material = material;
        }
    }
}
