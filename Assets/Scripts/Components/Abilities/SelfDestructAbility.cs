using TagsUtility;
using Unity.Mathematics;
using UnityEngine;

public class SelfDestructAbility : CollisionAbility, ICollisionAbility
{
    private string _targetTag;
    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public void Execute()
    {
        TrySelfDestruct();
    }

    private void TrySelfDestruct()
    {
        foreach (Collider target in Collisions)
        {
            _targetTag = target?.gameObject?.tag;
            if (!TagSelectorPropertyDrawer.TagSelectorContainsTag(TargetTags, _targetTag))
            {
                continue;
            }
            DestoySelf();
        }
    }

    private void DestoySelf()
    {
        Active = false;
    }
}