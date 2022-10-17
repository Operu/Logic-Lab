using System.Collections;
using System.Collections.Generic;
using Components;
using Components.Types;
using Systems;
using UnityEngine;

namespace Managers
{
    public class SimulationManager : MonoBehaviour
    {
        public Transform componentStorage;
        
        public float tps = 0.5f;

        public bool active = true;

        private List<BaseComponent> logicComponents;
        private List<Pin> outputPins;

        public bool TryAddToSimulation(Transform component)
        {
            if (component.CompareTag("Component"))
            {
                logicComponents.Add(component.GetComponent<BaseComponent>());
                foreach (Transform pin in component)
                {
                    if (pin.CompareTag("Pin"))
                    {
                        Pin componentPin = pin.GetComponent<Pin>();
                        if (componentPin.pinType == PinType.OUTPUT)
                            outputPins.Add(componentPin);
                    }
                }
                return true;
            }
            return false;
        }
        
    
        private void Start()
        {
            logicComponents = new List<BaseComponent>();
            outputPins = new List<Pin>();
        
            foreach (Transform child in componentStorage)
            {
                if (!TryAddToSimulation(child)) Debug.LogError("Error adding " + child.name);
            }
            Debug.Log("Initialized Components - Starting Simulation");
            StartCoroutine(SimulationLoop());
        }

        private IEnumerator SimulationLoop()
        {
            float delay = 1 / tps;
            while (active)
            {
                Debug.Log("1 tick");
                // First every wire is set to off state
                
                // Loop should start with updating all component states.
                // This is done with the logic update, for all logic components.
                
                // After that we make all output pins evaluate their state and broadcast that to their connections.
                // every wire receiving that broadcast will evaluate their state and set internal bool "hasEvaluated" to true.
                // If that wire is again forced to evaluate state it will realised it has already evaluated and it will stop potential wire loops.

                // At last all wires will be told to update their visual state and change material.

                foreach (BaseComponent component in logicComponents)
                {
                    // Update component state
                    component.ComponentUpdate();
                }

                foreach (Pin outputPin in outputPins)
                {
                    // Evaluate output pin state
                    outputPin.EvaluateState();
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
