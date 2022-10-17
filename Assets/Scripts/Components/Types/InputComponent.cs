using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class InputComponent : BaseComponent
    {
        public List<Pin> outputs;

        protected abstract void LogicUpdate();

        public override void ComponentUpdate()
        {
            LogicUpdate();
        }

        public void Connect(Wire connection)
        {
            
        }
    }
}