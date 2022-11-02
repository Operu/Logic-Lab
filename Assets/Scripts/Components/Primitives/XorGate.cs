using Components.Types;

namespace Components.Primitives
{
    public class XorGate : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = inputs[0].State ^ inputs[1].State;
        }
    }
}
