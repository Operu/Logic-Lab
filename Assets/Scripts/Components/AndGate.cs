using System.Collections.Generic;
using System.Transactions;
using Components.Types;
using Systems;

namespace Components
{
    public class AndGate : LogicComponent
    {
        protected override void LogicUpdate()
        {
            State = inputs[0].State && inputs[1].State;
        }
    }
}
