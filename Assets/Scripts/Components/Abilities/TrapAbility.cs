using TagsUtility;
using UnityEngine;

public class TrapAbility : CollisionAbility, ICollisionAbility
{
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float damageDuration = 0.5f;
    private IHealth _targetHealth;
    private string _targetTag;
    private float _damageTimer = 0;
    private float _timeCurrent;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        _timeCurrent = Time.time;
    }

    public void Execute()
    {
        _timeCurrent = Time.time;
        if ((_timeCurrent - _damageTimer) > damageDuration)
        {
            _damageTimer = _timeCurrent;
            TryDealDamage();
        }
    }

    private void TryDealDamage()
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
                _targetHealth.TakeDamage(damage);
            }
        }
    }
}