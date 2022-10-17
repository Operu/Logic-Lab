using System;
using Components.Types;
using UnityEngine;

namespace Components
{
    public class Lever : InputComponent
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Utility.GridMousePos() == (Vector2)transform.position) State = !State;
        }
        
        protected override void LogicUpdate()
        {
            
        }
    }
}
