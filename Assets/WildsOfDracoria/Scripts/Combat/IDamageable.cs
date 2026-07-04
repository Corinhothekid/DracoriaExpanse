namespace WildsOfDracoria.Combat
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        bool IsDead();
    }
}
