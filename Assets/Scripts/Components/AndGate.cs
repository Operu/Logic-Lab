using System.Collections.Generic;

namespace Components
{
    public class AndGate : LogicComponent
    {
        // Start is called before the first frame update
        void Start()
        {
            inputs = new List<bool>(2);
            outputs = new List<bool>(1);
        }

        protected override void LogicUpdate()
        {
        
            outputs[0] = inputs[0] && inputs[1];
        }
    }
}
