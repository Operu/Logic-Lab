using System.Collections.Generic;
using System.Linq;
using Components.Types;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace Systems
{
    public class Pin : BaseComponent
    {
        public BaseComponent parent;
        public PinType pinType;
        public bool State { get; private set; }

        public List<Wire> connections;
        
        public void ConnectWire(Wire wire)
        {
            connections.Add(wire);
        }

        public void EvaluateState()
        {
            if (pinType == PinType.INPUT)
            {
                if (connections.Any(input => input.State))
                {
                    State = true;
                }
            }

            if (pinType == PinType.OUTPUT)
            {
                State = parent.State;
            }
        }
        
        
    }

    public enum PinType
    {
        UNDEFINED,
        INPUT,
        OUTPUT
    }
} 