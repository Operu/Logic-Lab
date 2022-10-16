using UnityEngine;

public static class Utility
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
 
    public static Vector2 GridMousePos()
    {
        return RoundVector2(MouseWorldPos());
    }
}