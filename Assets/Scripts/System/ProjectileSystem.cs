using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileSystem : ComponentSystem
{
    private EntityQuery _projectilesQuerry;
    private float3 _newPosition;
    private float _deltaTime;

    protected override void OnCreate()
    {
        _projectilesQuerry = GetEntityQuery(
            ComponentType.ReadOnly<Projectile>(),
            ComponentType.ReadOnly<Transform>(),
            ComponentType.ReadOnly<ProjectileData>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        Entities.With(_projectilesQuerry).ForEach
            (
                (ref ProjectileData projectileData, Transform transform, Projectile projectile) =>
                {
                    ManageProjectileTravel(transform, projectile, ref projectileData, _deltaTime);
                }
            );
    }

    private void ManageProjectileTravel(Transform transform, Projectile projectile, ref ProjectileData projectileData, float deltaTime)
    {
        if (projectile.Active)
        {
            _newPosition = transform.forward * projectileData.Speed * deltaTime;
            transform.position += new Vector3(_newPosition.x, _newPosition.y, _newPosition.z);
        }
    }
}
