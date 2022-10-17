using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class LogicComponent : BaseComponent
    {
        public List<Pin> inputs;
        public List<Pin> outputs;

        public abstract void LogicUpdate();

        public void ComponentUpdate()
        {
            LogicUpdate();
        }

        public void Connect(Wire connection)
        {
            
        }
    }
}
