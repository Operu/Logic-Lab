using System.Collections.Generic;
using System.Linq;
using Components.Types;
using Systems;
using UnityEngine;

namespace Player.Tools
{
    public class DestroyTool : MonoBehaviour
    {
        [Header("Extern References")]
        [SerializeField] private WireTool wireTool;
        [SerializeField] private SelectTool selection;

        public void DestroyObject()
        {
            
            GameObject subjectComponent = selection.GetComponentOnMouse();
            if (subjectComponent)
            {
                Destroy(subjectComponent);
                return;
            }
            
            foreach (WireInterface wireInterface in selection.selectedWireInterfaces)
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
                    wire.Destroy();
                    selection.ImmediateReUpdate();
                }
                return;
            }
            
        }
    }
}