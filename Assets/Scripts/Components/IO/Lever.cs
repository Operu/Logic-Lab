using Components.Types;
using UnityEngine;
using Utilities;

namespace Components.IO
{
    public class Lever : InputComponent
    {
        [SerializeField] private SpriteRenderer lamp;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Helpers.PreciseGridMousePos() == (Vector2)transform.position)
            {
                State = !State;
                lamp.color = State ? new Color32(255, 0, 0, 255) : new Color32(29, 31, 35, 255);
            }
        }
        
        protected override void LogicUpdate()
        {
            
        }
    }
}
