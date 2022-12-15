using System;
using Components.Types;
using UnityEngine;
using Utilities;

namespace Components
{
    public class Lever : InputComponent
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Helpers.PreciseGridMousePos() == (Vector2)transform.position)
            {
                State = !State;
            }
        }
        
        protected override void LogicUpdate()
        {
            
        }
    }
}
