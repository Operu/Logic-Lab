using System.Collections.Generic;
using System.Linq;
using Components.Types;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Tools
{
    public class DestroyTool : MonoBehaviour
    {
        public bool active;

        [Header("Extern References")]
        [SerializeField] private WiringTool wiringTool;
        [SerializeField] private PlayerSelection selection;
        
        public void DestroyInput(InputAction.CallbackContext context)
        {
            if (context.started) DestroyObjects();
        }

        private void DestroyObjects()
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
                        wiringTool.AddWireConnections(connection);
                    }
                    wire.Destroy();
                    selection.ImmediateReUpdate();
                }
                return;
            }
            
        }
    }
}