public interface IHealth
{
    float Health { get; }
    void HealUp(float healValue);
    void TakeDamage(float damageValue);

}
