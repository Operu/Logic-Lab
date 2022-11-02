using Components.Types;

namespace Components.Primitives
{
    public class AndGate : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = inputs[0].State && inputs[1].State;
        }
    }
}
