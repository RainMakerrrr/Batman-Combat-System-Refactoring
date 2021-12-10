using System.Collections.Generic;
using UnityEngine;

namespace Player.Animations
{
    public class AnimatorStateReader : MonoBehaviour, IAnimatorStateReader
    {
        private readonly int _movement = Animator.StringToHash("Movement");
        private readonly int _strafe = Animator.StringToHash("Strafe");
        private readonly int _flipKick = Animator.StringToHash("Flip Kick");
        private readonly int _flyingKick = Animator.StringToHash("Flying Kick");
        private readonly int _crescentKick = Animator.StringToHash("Crescent Kick");
        private readonly int _flyingPunchCombo = Animator.StringToHash("Flying Punch Combo");
        private readonly int _crossPunch = Animator.StringToHash("Cross Punch");
        private readonly int _dodging = Animator.StringToHash("Dodging");
        private readonly int _getHit = Animator.StringToHash("Hit");
        private readonly int _death = Animator.StringToHash("Death");
        
        private Dictionary<int, AnimatorState> _states;
        public AnimatorState CurrentState { get; private set; }

        private void Start()
        {
            _states = new Dictionary<int, AnimatorState>
            {
                {_movement, AnimatorState.Move},
                {_strafe, AnimatorState.Move},
                {_flipKick, AnimatorState.Attack},
                {_flyingKick, AnimatorState.Attack},
                {_crescentKick, AnimatorState.Attack},
                {_flyingPunchCombo, AnimatorState.Attack},
                {_crossPunch, AnimatorState.Attack},
                {_dodging, AnimatorState.Attack},
                {_getHit, AnimatorState.Hit},
                {_death, AnimatorState.Death}
            };
        }

        public void Enter(int shortNameHash)
        {
            CurrentState = TryGetStateByHash(shortNameHash);
        }

        public void Exit(int shortNameHash)
        {
        }

        private AnimatorState TryGetStateByHash(int stateHash) =>
            _states.TryGetValue(stateHash, out AnimatorState state) ? state : CurrentState;
    }
}