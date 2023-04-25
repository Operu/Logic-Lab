using System;
using Player.Tools;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public enum PlayerState
    {
        IDLE, // When nothing else is being done.
        INTERACTING, // When interacting with objects, like clicking a button
        PLACING, // When you are placing an object.
        WIRING, // When you are dragging a wire
        PAUSED // When you press "Esc" and pause the game
    }
    
    public class PlayerManager : Singleton<PlayerManager>
    {
        
        public Vector3 MousePosition { get; private set; }
        public bool IsHoveringDeletable { get; private set; }
        public bool IsHoveringConnectable { get; private set; }
        
        public PlayerState State { get; private set; }

        [SerializeField] private DestroyTool destroyTool;
        [SerializeField] private PlaceTool placeTool;
        [SerializeField] private WireTool wireTool;
        [SerializeField] private SelectTool selectTool;

        [SerializeField] private PlayerMouse mouse;
        
        private new void Awake()
        {
            State = PlayerState.IDLE;
        }

        private void UpdateMousePosition(Vector2 mousePos)
        {
            MousePosition = mousePos;
            switch (State)
            {
                case PlayerState.IDLE:
                    selectTool.UpdateMousePos(mousePos);
                    break;
                case PlayerState.INTERACTING:
                    break;
                case PlayerState.PLACING:
                    placeTool.UpdatePlacingPosition(mousePos);
                    break;
                case PlayerState.WIRING:
                    wireTool.UpdateMousePosition(mousePos);
                    break;
                case PlayerState.PAUSED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetState(PlayerState newState)
        {
            State = newState;
        }

        public void InteractInput(InputAction.CallbackContext context)
        {
            PlayerState newState = State;
            
            if (State is PlayerState.PLACING && context.started)
            {
                placeTool.PlaceComponent();
                newState = PlayerState.IDLE;
            }

            if (State is PlayerState.IDLE && context.started)
            {
                wireTool.StartWire();
                newState = PlayerState.WIRING;
            }

            if (State is PlayerState.WIRING && context.canceled)
            {
                wireTool.StopWire();
                newState = PlayerState.IDLE;
            }
            
            SetState(newState);
        }

        public void RotateInput(InputAction.CallbackContext context)
        {
            if (State is PlayerState.PLACING && context.started)
            {
                placeTool.RotateComponent();
            }
        }

        public void DestroyInput(InputAction.CallbackContext context)
        {
            if (State is PlayerState.IDLE && context.started)
            {
                destroyTool.DestroyObject();    
            }
        }

        public void PauseInput(InputAction.CallbackContext context)
        {
            if (State is PlayerState.WIRING) wireTool.StopWire();
            if (State is PlayerState.PLACING) placeTool.PlaceComponent();
            
            SetState(PlayerState.PAUSED);
        }

        public void UIButtonInput(PlacementItem item)
        {
            if (State is PlayerState.IDLE)
            {
                SetState(PlayerState.PLACING);
                placeTool.SelectComponent(item);
            }
        }
    }
}