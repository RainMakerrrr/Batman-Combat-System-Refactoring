using Enemies.Animations;
using UnityEngine;

namespace Enemies.States
{
    public class IdleState : IState
    {
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly CharacterController _characterController;

        public IdleState(IEnemyAnimator enemyAnimator, CharacterController characterController)
        {
            _enemyAnimator = enemyAnimator;
            _characterController = characterController;
        }

        public void Enter()
        {
            _enemyAnimator.PlayIdle();
            _characterController.Move(Vector3.zero);
        }

        public void Tick()
        {
        }

        public void Exit()
        {
        }
    }
}