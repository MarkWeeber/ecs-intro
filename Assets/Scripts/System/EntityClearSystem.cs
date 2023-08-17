using Unity.Entities;
using UnityEngine;

public class EntityClearSystem : ComponentSystem
{
    private EntityQuery _entityQuery;
    private IDestructible _destructible;

    protected override void OnCreate()
    {
        _entityQuery = GetEntityQuery(ComponentType.ReadOnly<Transform>());
    }

    protected override void OnUpdate()
    {
        Entities.With(_entityQuery).ForEach((Entity entity, Transform transform) =>
            {
                ManageEntityDestruct(entity, transform);
            }
        );
    }

    private void ManageEntityDestruct(Entity entity, Transform transform)
    {
        _destructible = transform.gameObject.GetComponent<IDestructible>();
        if (_destructible != null)
        {
            if (!_destructible.Active)
            {
                EntityManager.DestroyEntity(entity);
                GameObject.Destroy(transform.gameObject);
            }
        }
    }
}