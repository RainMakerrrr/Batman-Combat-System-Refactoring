namespace Player.Animations
{
    public interface IAnimatorStateReader
    {
        public AnimatorState CurrentState { get; }
        void Enter(int shortNameHash);
        void Exit(int shortNameHash);
    }
}