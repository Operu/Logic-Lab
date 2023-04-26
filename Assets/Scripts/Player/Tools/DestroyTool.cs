using System.Collections.Generic;
using System.Linq;
using Managers;
using Systems;
using UnityEngine;

namespace Player.Tools
{
    public class DestroyTool : MonoBehaviour
    {
        [Header("Extern References")]
        [SerializeField] private WireTool wireTool;
        [SerializeField] private SelectTool selectTool;

        public void TryDestroyObject()
        {
            
            GameObject subjectComponent = selectTool.GetComponentOnMouse();
            if (subjectComponent)
            {
                SimulationManager.Instance.RemoveFromSimulation(subjectComponent.transform);
                Destroy(subjectComponent);
                return;
            }
            
            foreach (WireInterface wireInterface in selectTool.selectedWireInterfaces)
            {
                Wire wire = wireInterface as Wire;
                bool isPin = wireInterface as Pin;

                if (wire && !isPin)
                {
                    List<Wire> connections = wire.connections.ToList();
                    wire.Disable();
                    
                    foreach (Wire connection in connections)
                    {
                        connection.SoftDestroy();
                        wireTool.AddWireConnections(connection);
                    }
                    SimulationManager.Instance.RemoveFromSimulation(wire.transform);
                    wire.Destroy();
                    selectTool.UpdateSelection();
                }
                return;
            }
            
        }
    }
}