using UnityEngine;

namespace Systems
{
    public class WirePart : MonoBehaviour
    {
        private Wire parent;

        public void Initialize(Wire parent)
        {
            this.parent = parent;
            
        }

        public void PositionLine(Vector3 origin, Vector3 destination)
        {
                
        }

        public void AdjustLine(Vector3 destinationChange)
        {
            
        }
        
    }
}