using TagsUtility;
using UnityEngine;

public class BounceAbilityPickup : CollisionAbility, ICollisionAbility
{
    private FireAbility _targetFireAbility;
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
            _targetTag = target?.gameObject?.tag;
            if (TagSelectorPropertyDrawer.TagSelectorContainsTag(TargetTags, _targetTag))
            {
                _targetFireAbility = target?.gameObject?.GetComponent<FireAbility>();
                if (_targetFireAbility != null)
                {
                    _targetFireAbility.SpecialFire = true;
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