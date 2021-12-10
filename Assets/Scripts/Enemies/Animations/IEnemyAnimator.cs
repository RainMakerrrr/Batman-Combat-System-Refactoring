namespace Enemies.Animations
{
    public interface IEnemyAnimator
    {
        void EnableStrafe();
        void DisableStrafe();
        void PlayIdle();
        void UpdateMovementAnimation(float currentSpeed);
        void PlayAttack();
        void PlayHit();
        void PlayDeath();
        void UpdateStrafeAnimation(float strafeSpeed);
    }
}