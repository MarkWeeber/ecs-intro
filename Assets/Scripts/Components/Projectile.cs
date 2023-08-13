using Unity.Entities;
using UnityEngine;

public class Projectile : MonoBehaviour, IConvertGameObjectToEntity
{
    public float Speed = 10f;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    public bool Active = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        Disable();
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<ProjectileData>(entity, new ProjectileData { Speed = Speed, CreatedTime = Time.time, Active = false });
    }

    public void Enable(Transform pos)
    {
        Active = true;
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        transform.parent = null;
        transform.SetPositionAndRotation(pos.position, Quaternion.LookRotation(pos.forward));
    }

    public void Disable()
    {
        Active = false;
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
    }
}

public struct ProjectileData : IComponentData
{
    public float Speed;
    public float CreatedTime;
    public bool Active;
}
