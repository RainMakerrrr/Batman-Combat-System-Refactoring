using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class DeathState : IState
    {
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly Enemy _enemy;
        private readonly CharacterController _characterController;

        public DeathState(IEnemyAnimator enemyAnimator, Enemy enemy, CharacterController characterController)
        {
            _enemyAnimator = enemyAnimator;
            _enemy = enemy;
            _characterController = characterController;
        }

        public void Enter()
        {
            _enemyAnimator.PlayDeath();
            _characterController.enabled = false;
            _enemy.enabled = false;
        }

        public void Tick()
        {
        }

        public void Exit()
        {
        }
    }
}