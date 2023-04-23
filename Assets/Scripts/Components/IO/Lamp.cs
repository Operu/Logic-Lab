using System;
using Components.Types;
using UnityEngine;

namespace Components.IO
{
    public class Lamp : OutputComponent
    {

        [SerializeField] private SpriteRenderer lamp;
        
        protected override void LogicUpdate()
        {
            State = inputs[0].State;
            Debug.Log(State);
            lamp.color = State ? new Color32(255, 170, 0, 255)  : new Color32(29, 31, 35, 255);
        }
    }
}