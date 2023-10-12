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

    public new void Execute()
    {
        TryHeal();
    }

    private void TryHeal()
    {
        foreach (Collider target in Collisions)
        {
            if (target == null)
            {
                continue;
            }
            _targetTag = target?.gameObject?.tag;
            if (TagSelectorPropertyDrawer.TagSelectorContainsTag(TargetTags, _targetTag))
            {
                _targetHealth = target?.gameObject?.GetComponent<IHealth>();
                if (_targetHealth != null)
                {
                    _targetHealth.HealUp(healAmount);
                    DestoySelf();
                }
                break;
            }
        }
    }

    private void DestoySelf()
    {
        Active = false;
    }
}