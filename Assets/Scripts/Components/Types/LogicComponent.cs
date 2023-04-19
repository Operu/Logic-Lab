using System.Collections.Generic;
using System.Linq;
using Systems;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Components.Types
{
    public abstract class LogicComponent : BaseComponent
    {
        public List<Pin> inputs;
        public List<Pin> outputs;

        protected abstract void LogicUpdate();


        public override void Initialize()
        {
            IO = inputs.Concat(outputs).ToList();
        }
        
        public override void ComponentUpdate()
        {
            foreach (Pin inputPin in inputs)
            {
                inputPin.EvaluateState();
            }
            LogicUpdate();
        }
    }
}
