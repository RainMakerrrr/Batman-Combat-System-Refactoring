using Services.Input;
using UnityEngine;
using Zenject;

namespace Player.Animations
{
    public class PlayerAnimator : MonoBehaviour
    {
        private const float SpeedSmoothTime = 0.1f;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int CrossPunch = Animator.StringToHash("CrossPunch");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int FlyingPunch = Animator.StringToHash("FlyingPunch");
        private static readonly int CrescentKick = Animator.StringToHash("CrescentKick");
        private static readonly int FlyingKick = Animator.StringToHash("FlyingKick");
        private static readonly int FlipKick = Animator.StringToHash("FlipKick");
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField] private Animator _animator;

        private IInputService _inputService;
        private int _attackAnimationsCounter;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Update() => UpdateMovementAnimation();

        private void UpdateMovementAnimation() => _animator.SetFloat(Speed, _inputService.Movement.magnitude,
            SpeedSmoothTime, Time.deltaTime);

        public void PlayDefaultAttackAnimation() => _animator.SetTrigger(CrossPunch);

        public void PlayAttackAnimation()
        {
            int[] attackAnimations =
            {
                FlyingPunch, CrescentKick, FlyingKick, FlipKick
            };

            int randomAnimation = attackAnimations[_attackAnimationsCounter % attackAnimations.Length];
            _animator.SetTrigger(randomAnimation);

            _attackAnimationsCounter++;
        }

        public void PlayDodgeAnimation() => _animator.SetTrigger(Dodge);

        public void PlayHitAnimation() => _animator.SetTrigger(Hit);
    }
}