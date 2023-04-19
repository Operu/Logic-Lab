using System.Collections.Generic;
using System.Linq;
using Systems;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Components.Types
{
    public abstract class InputComponent : BaseComponent
    {
        public List<Pin> outputs;

        protected abstract void LogicUpdate();


        public override void Initialize()
        {
            IO = outputs.ToList();
        }
        
        public override void ComponentUpdate()
        {
            LogicUpdate();
        }
    }
}