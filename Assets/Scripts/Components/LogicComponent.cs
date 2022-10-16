using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Components
{
    public abstract class LogicComponent : MonoBehaviour, IObject
    {

        public List<Wire> inputConnections;
    
        public List<bool> inputs;
        public List<bool> outputs;

        protected abstract void LogicUpdate();

        public void ComponentUpdate()
        {
            LogicUpdate();
        }

        public void Connect(IObject connection)
        {
            
        }
    }
}
