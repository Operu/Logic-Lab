using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace User.Tools
{
    public class DestroyTool : MonoBehaviour
    {
        [SerializeField] private WiringTool wiringTool;
        [SerializeField] private ObjectInteraction interaction;
        
        public void DestroyInput(InputAction.CallbackContext context)
        {
            if (context.started) DestroyObjects();
        }

        private void DestroyObjects()
        {
            foreach (WireInterface wireInterface in interaction.selectedWireInterfaces)
            {
                Wire wire = wireInterface as Wire;
                if (wire)
                {
                    List<Wire> connections = wire.connections;
                    wire.Destroy();
                    interaction.ImmediateReUpdate();

                    foreach (Wire connection in connections)
                    {
                        connection.SoftDestroy();
                        wiringTool.AddWireConnections(connection);
                    }
                    
                    return;
                }
            }
        }
    }
}