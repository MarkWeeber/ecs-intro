using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileSystem : ComponentSystem
{
    private EntityQuery _projectilesQuerry;
    private float3 _newPosition;
    private float _deltaTime;
    private float _currentTime;

    protected override void OnCreate()
    {
        _projectilesQuerry = GetEntityQuery(
            ComponentType.ReadOnly<ProjectileData>(),
            ComponentType.ReadOnly<Transform>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        _currentTime = UnityEngine.Time.time;
        Entities.With(_projectilesQuerry).ForEach
            (
                (Transform transform, ref ProjectileData projectileData) =>
                {
                    ManageProjectileTravel(transform, ref projectileData, _deltaTime, _currentTime);
                }
            );
    }

    private void ManageProjectileTravel(Transform transform, ref ProjectileData projectileData, float deltaTime, float currentTime)
    {
        //if (projectileData.CreatedTime + projectileData.LifeTime > currentTime)
        //{
        //    GameObject.Destroy(transform.gameObject);
        //}
        //else
        {
            _newPosition = transform.forward * projectileData.Speed * deltaTime;
            transform.position = _newPosition;
        }
        
    }
}
