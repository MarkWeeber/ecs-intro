using UnityEngine;

public class CharacterHealth : MonoBehaviour, IHealth
{
    [SerializeField]
    private float health = 100f;
    public float Health { get { return health; } }

    public void HealUp(float healValue)
    {
        health += healValue;
    }

    public void TakeDamage(float damageValue)
    {
        health -= damageValue;
    }
}