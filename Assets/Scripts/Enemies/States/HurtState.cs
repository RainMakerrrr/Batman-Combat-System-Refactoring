using DG.Tweening;
using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class HurtState : IState
    {
        private const float Delay = 0.1f;
        private const float Duration = 0.3f;
        
        private readonly EnemyHealth _enemyHealth;
        private readonly CharacterController _characterController;
        private readonly IEnemyAnimator _enemyAnimator;

        public HurtState(EnemyHealth enemyHealth, CharacterController characterController, IEnemyAnimator enemyAnimator)
        {
            _enemyHealth = enemyHealth;
            _characterController = characterController;
            _enemyAnimator = enemyAnimator;
        }

        public void Enter()
        {
            _enemyHealth.TakeDamage();
            _characterController.Move(Vector3.zero);
            _enemyAnimator.UpdateMovementAnimation(0f);

            _characterController.transform
                .DOMove(_characterController.transform.position - _characterController.transform.forward / 2f, Duration)
                .SetDelay(Delay);
        }

        public void Tick()
        {
        }

        public void Exit()
        {
        }
    }
}