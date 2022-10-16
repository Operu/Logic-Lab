using UnityEngine;

namespace Managers
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance { get; private set; }

        public Color pinHighlight = new Color(178, 178, 178);
        public Color pinDefault = new Color(0, 0, 0);

        public GameObject stubPrefab;
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
