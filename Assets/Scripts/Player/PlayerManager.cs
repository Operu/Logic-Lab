using System;
using Player.States;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        private PlayerBaseState currentState;
        
        private PlayerIdleState idleState = new PlayerIdleState();
        private PlayerWiringState wiringState = new PlayerWiringState();
        private PlayerPlacingState placingState = new PlayerPlacingState();
        private PlayerPausedState pausedState = new PlayerPausedState();
        
        
        private void Start()
        {
            currentState = idleState;
        }

        private void Update()
        {
            
        }
    }
}