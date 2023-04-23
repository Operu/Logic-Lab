using System.Collections.Generic;
using System.Linq;
using Systems;

namespace Components.Types
{
    public abstract class OutputComponent : BaseComponent
    {
        public List<Pin> inputs;

        protected abstract void LogicUpdate();


        public override void Initialize()
        {
            IO = inputs.ToList();
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