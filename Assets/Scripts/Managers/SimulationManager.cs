using System.Collections;
using System.Collections.Generic;
using Components;
using Systems;
using UnityEngine;

namespace Managers
{
    public class SimulationManager : MonoBehaviour
    {
        public float tps = 0.5f;

        public bool active = true;
    
        private Transform storage;

        private List<Transform> components;
        private List<Transform> wires;
    
    
        private void Start()
        {
            storage = GameObject.FindWithTag("Storage").transform;
            Debug.Log(storage.tag);

            components = new List<Transform>();
            wires = new List<Transform>();
        
            foreach (Transform child in storage)
            {
                Debug.Log(child.name);
                if (child.CompareTag("Component")) components.Add(child);
                if (child.CompareTag("Wire")) wires.Add(child);
            }
            Debug.Log(components.Count + " " + wires.Count);
        
            Debug.Log("Initialized Components - Starting Simulation");
            StartCoroutine(SimulationLoop());
        }

        private IEnumerator SimulationLoop()
        {
            float delay = 1 / tps;
            while (active)
            {
                foreach (Transform component in components)
                {
                    if((gameObject.GetComponent<LogicComponent>() as LogicComponent) == null) continue;
                    component.GetComponent<LogicComponent>().ComponentUpdate();
                }

                foreach (Transform wire in wires)
                {
                    wire.GetComponent<Wire>().StateUpdate();
                }

                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
}
