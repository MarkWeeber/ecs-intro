using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using EcsCollisionUtility;

public class ProjectileSystem : ComponentSystem
{
    private EntityQuery _rigidObjectsQuery;
    private float3 _newPosition;
    private float _deltaTime;
    private float3 _targetRotation;
    private Quaternion _rotation;

    protected override void OnCreate()
    {
        _rigidObjectsQuery = GetEntityQuery(
            ComponentType.ReadOnly<Projectile>(),
            ComponentType.ReadOnly<Transform>(),
            ComponentType.ReadOnly<RigidObjectData>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        Entities.With(_rigidObjectsQuery).ForEach
            (
                (ref RigidObjectData rigidObjectData, Transform transform, Projectile projectile) =>
                {
                    ManageProjectileTravel(transform, projectile, ref rigidObjectData, _deltaTime);
                }
            );
    }

    private void ManageProjectileTravel(Transform transform, Projectile projectile, ref RigidObjectData rigidObjectData, float deltaTime)
    {
        if (projectile.ActiveProjectile)
        {
            rigidObjectData.Velocity = transform.TransformVector(rigidObjectData.ForwardDirectionSelf).normalized;
            _newPosition = (float3)projectile.transform.position + rigidObjectData.Velocity * projectile.InitialSpeed * deltaTime;
            transform.position = _newPosition;
            transform.rotation = Quaternion.LookRotation(rigidObjectData.Velocity);
            if (math.length(rigidObjectData.ForwardDirectionSelf - rigidObjectData.ForwardInitialDirectionSelf) > 0.0001f)
            {
                rigidObjectData.ForwardDirectionSelf = rigidObjectData.ForwardInitialDirectionSelf;
            }
        }
    }
}
