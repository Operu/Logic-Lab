using System.Collections;
using System.Collections.Generic;
using Components;
using Components.Types;
using Systems;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SimulationManager : Singleton<SimulationManager>
    {

        public Transform componentStorage;
        
        public float tps = 0.5f;

        public bool active = true;

        private int tickNumber;

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
                tickNumber += 1;

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
        
    }
}
