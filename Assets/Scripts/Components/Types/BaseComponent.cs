using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public bool State { get; protected set; }

        public abstract void ComponentUpdate();
    }
}