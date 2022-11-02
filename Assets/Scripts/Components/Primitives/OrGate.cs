using Components.Types;

namespace Components.Primitives
{
    public class OrGate : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = inputs[0].State || inputs[1].State;
        }
    }
}
