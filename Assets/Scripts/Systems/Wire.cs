using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Systems
{
    public class Wire : MonoBehaviour
    {
        public bool state = false;
    

        public List<LogicComponent> connections;
    
        public void StateUpdate()
        {
        
        
            //Update wire state 
        }
    }
}
