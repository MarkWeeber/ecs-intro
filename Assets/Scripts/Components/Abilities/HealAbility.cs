using TagsUtility;
using UnityEngine;

public class HealAbility : CollisionAbility, ICollisionAbility
{
    [SerializeField]
    private float healAmount = 20f;
    private IHealth _targetHealth;
    private string _targetTag;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public void Execute()
    {
        TryHeal();
    }

    private void TryHeal()
    {
        foreach (Collider target in Collisions)
        {
            _targetTag = target?.gameObject?.tag;
            if (!TagSelectorPropertyDrawer.TagSelectorContainsTag(TargetTags, _targetTag))
            {
                continue;
            }
            _targetHealth = target?.gameObject?.GetComponent<IHealth>();
            if (_targetHealth != null)
            {
                _targetHealth.HealUp(healAmount);
                DestoySelf();
            }
        }
    }

    private void DestoySelf()
    {
        Active = false;
    }
}