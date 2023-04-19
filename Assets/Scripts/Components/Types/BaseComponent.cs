using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public bool State { get; protected set; }
        
        public List<Pin> IO { get; protected set; }

        public abstract void Initialize();
        public abstract void ComponentUpdate();
    }
}