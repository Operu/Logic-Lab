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

        public GameObject stub;
        public GameObject intersection;

        public List<Wire> connections;

        private SpriteRenderer stubRenderer;

        private bool hasEvaluated;
        private LineRenderer wireRenderer;
        private EdgeCollider2D wireCollider;

        public override void ConnectWire(Wire wire)
        {
            connections.Add(wire);
        }
        
        public void Initialize(List<Vector3> wirePoints)
        {
            wireRenderer = GetComponent<LineRenderer>();
            wireCollider = GetComponent<EdgeCollider2D>();
            
            
            List<Vector2> colliderPoints = new List<Vector2>();
            wireRenderer.positionCount = wirePoints.Count;
            for (int i = 0; i < wireRenderer.positionCount; i++)
            {
                wireRenderer.SetPosition(i, wirePoints[i]);
                colliderPoints.Add(wirePoints[i]);
            }
            wireCollider.SetPoints(colliderPoints);

            stub = Instantiate(Manager.Instance.stubPrefab, transform);
            stub.transform.position = wireRenderer.GetPosition(wireRenderer.positionCount - 1);
            stubRenderer = stub.GetComponent<SpriteRenderer>();
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

        public void VisualUpdate()
        {
            Material material = State ? Manager.Instance.wireOn : Manager.Instance.wireOff;
            wireRenderer.material = material;
            stubRenderer.material = material;
        }
    }
}
