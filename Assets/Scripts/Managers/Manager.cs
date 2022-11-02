using UnityEngine;

namespace Managers
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance { get; private set; }

        public Material wireOff;
        public Material wireOn;
        
        public GameObject intersectionPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }
    }
}
