using System.Collections.Generic;
using System.Linq;
using Components.Types;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace Systems
{
    public class Pin : WireInterface
    {
        public BaseComponent parent;
        public PinType pinType;

        public List<Wire> connections;
        
        public override void ConnectWire(Wire wire)
        {
            if (connections.Contains(wire)) return;
            connections.Add(wire);
        }

        public void EvaluateState()
        {
            if (pinType == PinType.INPUT)
            {
                State = connections.Any(wire => wire.State);
            }

            if (pinType == PinType.OUTPUT)
            {
                State = parent.State;
                if (!State) return;
                foreach (Wire wire in connections)
                {
                    wire.ActivateState();
                }
            }
        }
        
        
    }

    public enum PinType
    {
        // Default
        UNDEFINED,
        // Pin that takes in INPUTS
        INPUT,
        // Pin that OUTPUTS
        OUTPUT
    }
} 