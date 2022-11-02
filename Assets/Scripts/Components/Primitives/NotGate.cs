using Components.Types;

namespace Components.Primitives
{
    public class NotGate : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = !inputs[0].State;
        }
    }
}
