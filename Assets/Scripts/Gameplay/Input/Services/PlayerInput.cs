using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RunnerShooter.Gameplay.Input
{
    public sealed class PlayerInput : IPlayerInput, IInitializable, IDisposable
    {
        private readonly PlayerInputActions m_Actions = new();
        private float m_PressStartX;

        public bool IsPressed => m_Actions.Gameplay.Press.IsPressed();
        public bool PressedThisFrame => m_Actions.Gameplay.Press.WasPressedThisFrame();
        public float DragDelta => IsPressed ? (m_Actions.Gameplay.Position.ReadValue<Vector2>().x - m_PressStartX) / Screen.width : 0f;

        public void Initialize()
        {
            m_Actions.Gameplay.Press.started += OnPressStarted;
            m_Actions.Gameplay.Enable();
        }

        public void Dispose()
        {
            m_Actions.Gameplay.Press.started -= OnPressStarted;
            m_Actions.Dispose();
        }

        private void OnPressStarted(InputAction.CallbackContext context)
        {
            m_PressStartX = ((Pointer)context.control.device).position.ReadValue().x;
        }
    }
}
