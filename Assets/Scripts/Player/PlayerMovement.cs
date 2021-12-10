using Data;
using Player.Animations;
using Services.Assets;
using Services.Input;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private AnimatorStateReader _stateReader;

        private IInputService _inputService;
        private IAssetProvider _assetProvider;
        private Camera _camera;

        private PlayerStats _playerStats;
        private float _verticalVelocity;

        private bool CanMove => _stateReader.CurrentState != AnimatorState.Attack &&
                                _stateReader.CurrentState != AnimatorState.Hit;

        [Inject]
        private void Construct(IInputService inputService, IAssetProvider assetProvider, Camera camera)
        {
            _inputService = inputService;
            _assetProvider = assetProvider;
            _camera = camera;
        }

        private void Start() => _playerStats = _assetProvider.LoadData<PlayerStats>(AssetPath.PlayerStats);

        private void Update()
        {
            Move();
            UpdateGravity();
        }

        private void Move()
        {
            if (CanMove == false) return;

            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * _inputService.Movement.y + right * _inputService.Movement.x;

            _characterController.Move(desiredMoveDirection *
                                      (Time.deltaTime * (_playerStats.MovementSpeed * _playerStats.Acceleration)));

            Rotate(desiredMoveDirection);
        }

        private void Rotate(Vector3 desiredMoveDirection)
        {
            if (desiredMoveDirection == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                _playerStats.RotationSpeed * Time.deltaTime);
        }

        private void UpdateGravity()
        {
            if (_characterController.isGrounded == false)
                _verticalVelocity -= 1;
            var gravity = new Vector3(0f, _verticalVelocity * _playerStats.FallSpeed * Time.deltaTime, 0f);

            _characterController.Move(gravity);
        }
    }
}