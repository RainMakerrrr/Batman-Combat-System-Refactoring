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
            _randomDirection = Random.Range(0, 2) == 1 ? Vector3.left : Vector3.right;
        }

        public void Tick()
        {
            LookAtPlayer();
            UpdateStrafeAnimation();
            Strafe();
        }

        public void Exit() => _enemyAnimator.DisableStrafe();

        private void UpdateStrafeAnimation() => _enemyAnimator.UpdateStrafeAnimation(_randomDirection.normalized.x);

        private void LookAtPlayer() => _characterController.transform.LookAt(_playerTransform);

        private void Strafe()
        {
            Vector3 direction = (_playerTransform.position - _characterController.transform.position).normalized;
            Vector3 perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * direction;

            Vector3 finalDirection = perpendicular * _randomDirection.normalized.x;

            _characterController.Move(finalDirection * (_enemyStats.StafeSpeed * Time.deltaTime));
        }
    }
}