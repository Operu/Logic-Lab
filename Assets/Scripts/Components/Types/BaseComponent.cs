using Systems;
using UnityEngine;

namespace Components.Types
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public  bool State { get; private set; }
    }
}