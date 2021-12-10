namespace Enemies.States
{
    public interface IState
    {
        void Enter();
        void Tick();
        void Exit();
    }
}