using Data;
using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class StrafeState : IState
    {
        private readonly CharacterController _characterController;
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly Transform _playerTransform;
        private readonly EnemyStats _enemyStats;

        private Vector3 _randomDirection;
        private float _strafeSpeed;

        public StrafeState(CharacterController characterController, IEnemyAnimator enemyAnimator,
            Transform playerTransform, EnemyStats enemyStats)
        {
            _characterController = characterController;
            _enemyAnimator = enemyAnimator;
            _playerTransform = playerTransform;
            _enemyStats = enemyStats;
        }

        public void Enter()
        {
            _enemyAnimator.EnableStrafe();
            Vector3[] directions =
            {
                Vector3.back, Vector3.left, Vector3.right
            };

            _randomDirection = directions[Random.Range(0, directions.Length)];
            _strafeSpeed = _randomDirection == Vector3.back ? _enemyStats.BackStafeSpeed : _enemyStats.StrafeSpeed;
        }

        public void Tick()
        {
            LookAtPlayer();
            UpdateStrafeAnimation();
            Strafe();
        }

        public void Exit()
        {
        }

        private void UpdateStrafeAnimation() => _enemyAnimator.UpdateStrafeAnimation(_randomDirection.normalized.x);

        private void LookAtPlayer() => _characterController.transform.LookAt(_playerTransform);

        private void Strafe()
        {
            Vector3 direction = (_playerTransform.position - _characterController.transform.position).normalized;
            Vector3 perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * direction;

            Vector3 finalDirection = _randomDirection == Vector3.back
                ? -direction
                : perpendicular * _randomDirection.normalized.x;

            _characterController.Move(finalDirection * (_strafeSpeed * Time.deltaTime));
        }
    }
}