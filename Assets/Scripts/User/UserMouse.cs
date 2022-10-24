using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace User
{
    [System.Serializable] public class MousePosChangedEvent : UnityEvent<Vector2> { }
    
    public class UserMouse : MonoBehaviour
    {
        public MousePosChangedEvent mousePosChangedEvent;

        private Vector2 gridMousePos;
        
        private void Update()
        {
            if (gridMousePos != Utility.PreciseGridMousePos())
            {
                gridMousePos = Utility.PreciseGridMousePos();
                mousePosChangedEvent.Invoke(gridMousePos); 
            }
        }
    }
}