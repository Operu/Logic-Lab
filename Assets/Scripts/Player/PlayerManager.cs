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
        public PlayerState State { get; private set; } = PlayerState.IDLE;

        [Header("Tools")]
        public DestroyTool destroyTool;
        public PlaceTool placeTool;
        public SelectTool selectTool;
        public WireTool wireTool;

        public void UpdateMousePosition(Vector2 mousePos)
        {
            MousePosition = mousePos;
            switch (State)
            {
                case PlayerState.IDLE:
                    selectTool.UpdateSelection();
                    break;
                case PlayerState.INTERACTING:
                    break;
                case PlayerState.PLACING:
                    placeTool.UpdatePlacingPosition();
                    break;
                case PlayerState.WIRING:
                    wireTool.UpdateWirePreview();
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

            if (State is PlayerState.IDLE && context.started && selectTool.IsHoveringConnectable())
            {
                wireTool.StartWirePreview();
                selectTool.CursorClicked();
                newState = PlayerState.WIRING;
            }

            if (State is PlayerState.WIRING && context.canceled)
            {
                wireTool.StopWirePreview();
                selectTool.DisableCursor();
                selectTool.CursorClicked();
                newState = PlayerState.IDLE;
            }
            
            UpdateMousePosition(MousePosition);
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
                destroyTool.TryDestroyObject();    
            }
        }

        public void PauseInput(InputAction.CallbackContext context)
        {
            if (State is PlayerState.WIRING) wireTool.StopWirePreview();
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