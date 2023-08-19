using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour, IConvertGameObjectToEntity
{
    public float InitialSpeed = 10f;
    public bool ActiveProjectile = false;
    private MeshRenderer meshRenderer;
    private Collider _collider;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        Disable();
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<ProjectileObjectData>(entity,
            new ProjectileObjectData { Velocity = float3.zero, ForwardInitialDirectionSelf = new float3(0, 0, 1f), ForwardDirectionSelf = new float3(0, 0, 1f) });
    }

    public void Enable(Transform pos)
    {
        transform.SetPositionAndRotation(pos.position, Quaternion.LookRotation(pos.forward));
        _collider.enabled = true;
        ActiveProjectile = true;
        meshRenderer.enabled = true;
        transform.parent = null;
    }

    public void Disable()
    {
        _collider.enabled = false;
        ActiveProjectile = false;
        meshRenderer.enabled = false;
    }
}

public struct ProjectileObjectData : IComponentData
{
    public float3 Velocity;
    public float3 ForwardInitialDirectionSelf;
    public float3 ForwardDirectionSelf;
}