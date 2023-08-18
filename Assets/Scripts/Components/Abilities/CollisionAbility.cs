using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using EcsCollisionUtility;
using System.Collections.Generic;
using TagsUtility;

[RequireComponent(typeof(Collider))]
public class CollisionAbility : MonoBehaviour, IConvertGameObjectToEntity, ICollisionAbility, IDestructible
{
    [TagSelector]
    public string[] targetTags = new string[] { };
    public float Duration { get { return 0; } set {; } }
    public string[] TargetTags { get { return targetTags; } set { targetTags = value; } }
    public List<Collider> Collisions { get; set; }
    public Collider Collider { get; set; }
    public AbilityType abilityType = AbilityType.ConstantEffector;
    public AbilityType AbilityType { get { return abilityType; } set { abilityType = value; } }
    public bool Active { get { return _active; } set { _active = value; } }
    private bool _active = true;
    public ActorColliderData _ActorColliderData;
    public ProjectileObjectData _projectileData;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        float3 position = gameObject.transform.position;
        switch (Collider)
        {
            case SphereCollider sphere:
                sphere.ToWorldSpaceSphere(out var spherCenter, out var sphereRadius);
                dstManager.AddComponentData<ActorColliderData>(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Sphere,
                    SphereCenter = spherCenter - position,
                    SphereRadius = sphereRadius,
                    InitialTakeff = true
                }
                );
                break;
            case CapsuleCollider capsule:
                capsule.ToWorldSpaceCapsule(out var capsuleStart, out var capsuleEnd, out var capsuleRadius);
                dstManager.AddComponentData<ActorColliderData>(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Capsule,
                    CapsuleStart = capsuleStart - position,
                    CapsuleEnd = capsuleEnd - position,
                    CapsuleRadius = capsuleRadius,
                    InitialTakeff = true
                }
                );
                break;
            case BoxCollider box:
                box.ToWorldSpaceBox(out var boxCenter, out var boxHalfExtents, out var boxOrientation);
                dstManager.AddComponentData<ActorColliderData>(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Box,
                    BoxCenter = boxCenter - position,
                    BoxHalfExtents = boxHalfExtents,
                    BoxOrientation = boxOrientation,
                    InitialTakeff = true
                }
                );
                break;
            default: break;
        }
        Collider.enabled = false;
    }

    public void Execute()
    {
        Debug.Log(gameObject.name + ": hit by abstract collision ability");
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
    //    switch (_ActorColliderData.ColliderType)
    //    {
    //        case ColliderType.Sphere:
    //            Gizmos.DrawSphere(
    //                ((float3)transform.position + _ActorColliderData.SphereCenter) ,
    //                _ActorColliderData.SphereRadius);
    //            break;
    //        case ColliderType.Capsule:
    //            var _point1 = _ActorColliderData.CapsuleStart + (float3)transform.position;
    //            var _point2 = _ActorColliderData.CapsuleEnd + (float3)transform.position;
    //            var _center = (_point1 + _point2) / 2f;
    //            _point1 = (float3)(transform.rotation * (_point1 - _center)) + _center;
    //            _point2 = (float3)(transform.rotation * (_point2 - _center)) + _center;
    //            Gizmos.DrawSphere(_point1, _ActorColliderData.CapsuleRadius);
    //            Gizmos.DrawSphere(_point2, _ActorColliderData.CapsuleRadius);
    //            break;
    //        case ColliderType.Box:
    //            Gizmos.DrawCube(
    //                ((float3)transform.position),
    //                _ActorColliderData.BoxHalfExtents * 2f);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}

public struct ActorColliderData : IComponentData
{
    public ColliderType ColliderType;
    public float3 SphereCenter;
    public float SphereRadius;
    public float3 CapsuleStart;
    public float3 CapsuleEnd;
    public float CapsuleRadius;
    public float3 BoxCenter;
    public float3 BoxHalfExtents;
    public quaternion BoxOrientation;
    public bool InitialTakeff;
}

public enum ColliderType
{
    Sphere = 0,
    Capsule = 1,
    Box = 2
}