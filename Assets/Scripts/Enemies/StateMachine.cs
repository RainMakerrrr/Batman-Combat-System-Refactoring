using System;
using System.Collections.Generic;
using System.Linq;
using Enemies.States;
using UnityEngine;

namespace Enemies
{
    public class StateMachine
    {
        private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private readonly List<Transition> _anyTransitions = new List<Transition>();
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);

        private List<Transition> _currentTransitions = new List<Transition>();

        public IState CurrentState { get; private set; }

        public void Tick()
        {
            Transition transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            // if (CurrentState != null)
            //     Debug.Log(CurrentState);
            //
            CurrentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (CurrentState == state) return;

            CurrentState?.Exit();
            CurrentState = state;

            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            CurrentState?.Enter();
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition) =>
            _anyTransitions.Add(new Transition(to, condition));

        private Transition GetTransition()
        {
            foreach (Transition anyTransition in _anyTransitions.Where(anyTransition => anyTransition.Condition()))
                return anyTransition;

            return _currentTransitions.FirstOrDefault(currentTransition => currentTransition.Condition());
        }

        private class Transition
        {
            public IState To { get; }

            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
    }
}