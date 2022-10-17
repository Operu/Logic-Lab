using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class LogicComponent : BaseComponent
    {
        public List<Pin> inputs;
        public List<Pin> outputs;

        protected abstract void LogicUpdate();

        public override void ComponentUpdate()
        {
            foreach (Pin inputPin in inputs)
            {
                inputPin.EvaluateState();
            }
            LogicUpdate();
        }

        public void Connect(Wire connection)
        {
            
        }
    }
}
