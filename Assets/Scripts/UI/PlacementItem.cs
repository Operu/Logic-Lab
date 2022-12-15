using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "Placement Item", menuName = "Game/Placement Item", order = 0)]
    public class PlacementItem : ScriptableObject
    {
        public string itemName;
        
        public Vector2 size;
        public Vector2 offset;
        
        public GameObject prefab;

    }
}