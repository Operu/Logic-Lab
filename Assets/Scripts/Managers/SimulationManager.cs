using System.Collections;
using System.Collections.Generic;
using Components;
using Components.Types;
using Systems;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class SimulationManager : Utilities.Singleton<SimulationManager>
    {
        
        [Header("Simulation")]
        public float tps = 10f;
        public bool active = true;
        
        [Header("Extern References")]
        public Transform componentStorage;
        public Material wireOffMaterial;
        public Material wireOnMaterial;

        
        private List<BaseComponent> logicComponents;
        private List<Pin> outputPins;
        private List<Wire> wires;

        private void Start()
        {
            logicComponents = new List<BaseComponent>();
            outputPins = new List<Pin>();
            wires = new List<Wire>();
        
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
                foreach (BaseComponent component in logicComponents)
                {
                    // Update component state
                    component.ComponentUpdate();
                }

                foreach (Wire wire in wires)
                {
                    // Reset all wires
                    wire.ResetState();
                }

                foreach (Pin outputPin in outputPins)
                {
                    // Evaluate output pin state
                    outputPin.EvaluateState();
                }

                yield return new WaitForSeconds(delay);
            }
        }

        public void AddWireToSimulation(Wire wire)
        {
            wires.Add(wire);
        }
        
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

        public void RemoveFromSimulation(Transform component)
        {
            if (component.CompareTag("Component"))
            {
                logicComponents.Remove(component.GetComponent<BaseComponent>());
                foreach (Transform pin in component)
                {
                    if (pin.CompareTag("Pin"))
                    {
                        Pin componentPin = pin.GetComponent<Pin>();
                        if (componentPin.pinType == PinType.OUTPUT)
                            outputPins.Remove(componentPin);
                    }
                }
            }

            if (component.CompareTag("Wire"))
            {
                wires.Remove(component.GetComponent<Wire>());
            }
            
        }
    }
}
