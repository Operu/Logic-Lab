using UnityEngine;

namespace Utilities
{
    public static class Helpers
    {
        private static Camera mainCamera;

        // Constructor
        private static Camera MainCamera {
            get {
                if (mainCamera == null) {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }
        
        public static Vector2 MouseWorldPos() 
        { 
            return MainCamera.ScreenToWorldPoint (Input.mousePosition);
        }

        public static Vector2 RoundVector2(Vector2 input)
        {
            return Vector2Int.RoundToInt(input);
        }

        public static Vector2 PreciseRoundVector2(Vector2 input)
        {
            return RoundVector2(input * 2) / 2;
        }
 
        public static Vector2 GridMousePos()
        {
            return RoundVector2(MouseWorldPos());
        }

        public static Vector2 PreciseGridMousePos()
        {
            return PreciseRoundVector2(MouseWorldPos());
        }


        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform) Object.Destroy(child.gameObject);
        }
        
    }
}