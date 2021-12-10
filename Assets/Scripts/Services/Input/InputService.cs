using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input
{
    public class InputService : IInputService, IDisposable
    {
        public event Action Attack;
        public event Action Counter;
        public Vector2 Movement => _playerControls.Player.Movement.ReadValue<Vector2>();
        public Vector2 Look => _playerControls.Player.Look.ReadValue<Vector2>();

        private readonly PlayerControls _playerControls;

        public InputService(PlayerControls playerControls)
        {
            _playerControls = playerControls;
            _playerControls.Enable();

            _playerControls.Player.Attack.started += OnAttackStarted;
            _playerControls.Player.Counter.started += OnCounterStarted;
        }

        private void OnAttackStarted(InputAction.CallbackContext context) => Attack?.Invoke();

        private void OnCounterStarted(InputAction.CallbackContext context) => Counter?.Invoke();

        public void Dispose()
        {
            _playerControls.Player.Attack.started -= OnAttackStarted;
            _playerControls.Player.Counter.started -= OnCounterStarted;

            _playerControls?.Dispose();
            _playerControls.Disable();
        }
    }
}