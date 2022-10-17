using UnityEngine;

namespace Systems
{
    public abstract class WireInterface : MonoBehaviour
    {
        public  bool State { get; protected set; }
        
        public abstract void ConnectWire(Wire wire);
    }
}