using Data;
using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class ChaseState : IState
    {
        private readonly CharacterController _characterController;
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly Transform _playerTransform;
        private readonly EnemyStats _enemyStats;

        public ChaseState(CharacterController characterController, IEnemyAnimator enemyAnimator,
            Transform playerTransform, EnemyStats enemyStats)
        {
            _characterController = characterController;
            _enemyAnimator = enemyAnimator;
            _playerTransform = playerTransform;
            _enemyStats = enemyStats;
        }

        public void Enter()
        {
        }

        public void Tick() => ChasePlayer();

        public void Exit()
        {
        }

        private void ChasePlayer()
        {
            Vector3 direction = (_playerTransform.position - _characterController.transform.position).normalized;
            _characterController.Move(direction * _enemyStats.MovementSpeed * Time.deltaTime);

            _enemyAnimator.UpdateMovementAnimation(_characterController.velocity.normalized.magnitude);

            RotateToPlayer(direction);
        }

        private void RotateToPlayer(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _characterController.transform.rotation = Quaternion.Slerp(_characterController.transform.rotation,
                targetRotation, _enemyStats.RotationSpeed * Time.deltaTime);
        }
    }
}