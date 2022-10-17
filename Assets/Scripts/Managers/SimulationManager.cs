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

        private List<LogicComponent> components;
        private List<Pin> outputPins;

        public bool TryAddToSimulation(Transform component)
        {
            if (component.CompareTag("Component"))
            {
                components.Add(component.GetComponent<LogicComponent>());
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

            components = new List<LogicComponent>();
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
                foreach (LogicComponent component in components)
                {
                    component.LogicUpdate();
                }

                foreach (Pin pin in outputPins)
                {
                }

                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
}
