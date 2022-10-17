using Components.Types;
using UnityEngine;

namespace Components
{
    public class Lever : InputComponent
    {

        protected override void LogicUpdate()
        {
            State = Input.GetKey(KeyCode.T);
        }
    }
}
