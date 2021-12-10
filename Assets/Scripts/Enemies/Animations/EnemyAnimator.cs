using UnityEngine;

namespace Enemies.Animations
{
    public class EnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int AirPunch = Animator.StringToHash("AirPunch");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Strafe = Animator.StringToHash("Strafe");
        private static readonly int StrafeDirection = Animator.StringToHash("StrafeDirection");

        private const float DampTime = 0.1f;

        [SerializeField] private Animator _animator;

        public void EnableStrafe() => _animator.SetBool(Strafe, true);
        public void DisableStrafe() => _animator.SetBool(Strafe, false);

        public void UpdateStrafeAnimation(float strafeSpeed) => _animator.SetFloat(StrafeDirection, strafeSpeed, DampTime, Time.deltaTime);

        public void PlayIdle() => _animator.SetFloat(Speed, 0f);

        public void UpdateMovementAnimation(float currentSpeed) =>
            _animator.SetFloat(Speed, currentSpeed, DampTime, Time.deltaTime);

        public void PlayAttack() => _animator.SetTrigger(AirPunch);

        public void PlayHit() => _animator.SetTrigger(Hit);

        public void PlayDeath() => _animator.SetTrigger(Death);
    }
}