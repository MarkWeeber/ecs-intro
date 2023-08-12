using Unity.Entities;
using UnityEngine;

public class Projectile : MonoBehaviour, IConvertGameObjectToEntity
{
    public float Speed = 10f;
    public float LifeTime = 5f;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<ProjectileData>(entity, new ProjectileData { Speed = Speed, LifeTime = LifeTime, CreatedTime = Time.time, Active = false });
    }
}

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float LifeTime;
    public float CreatedTime;
    public bool Active;
}
