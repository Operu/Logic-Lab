using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Player
{
    [System.Serializable] public class MousePosChangedEvent : UnityEvent<Vector2> { }
    
    public class UserMouse : MonoBehaviour
    {
        public MousePosChangedEvent mousePosChangedEvent;

        private Vector2 gridMousePos;
        private Vector3 lastMousePos;
        
        private void Update()
        {
            if (lastMousePos != Input.mousePosition)
            {
                lastMousePos = Input.mousePosition;
                MousePositionChanged();
            }
        }

        private void MousePositionChanged()
        {
            Vector2 currentPos = Helpers.PreciseGridMousePos();
            if (gridMousePos != currentPos)
            {
                gridMousePos = currentPos;
                mousePosChangedEvent.Invoke(gridMousePos); 
            }
        }
    }
}