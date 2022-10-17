using System.Collections.Generic;
using Components;
using Components.Types;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems
{
    public class Wire : BaseComponent
    {

        public GameObject stub;
        public GameObject intersection;

        private LineRenderer wireRenderer;
        private EdgeCollider2D wireCollider;
        
        public List<BaseComponent> connections;

        public bool State { get; set; }
        
        public void Initialize(List<Vector3> wirePoints)
        {
            wireRenderer = GetComponent<LineRenderer>();
            wireCollider = GetComponent<EdgeCollider2D>();

            wireRenderer.sortingLayerName = "Foreground";
            
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
        }
        
        
        public void StateUpdate()
        {
            foreach (BaseComponent connection in connections)
            {

            }
        }
    }
}
