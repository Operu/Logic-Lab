using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace User.Tools
{
    public class DestroyTool : MonoBehaviour
    {
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
                    wire.Destroy();
                    interaction.ImmediateReUpdate();
                    return;
                }
            }
        }
    }
}