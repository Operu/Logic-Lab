using Components.Types;

namespace Components.Primitives
{
    public class Buffer : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = inputs[0].State;
        }
    }
}
