using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using EcsIntro;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class CollisionAbility : MonoBehaviour, IConvertGameObjectToEntity, ICollisionAbility
{
    [TagSelector]
    public string[] TargetTags = new string[] { };
    public float Duration { get { return 0; } set {; } }
    public List<Collider> Collisions { get; set; }
    public Collider Collider { get; set; } 

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
        Debug.Log(gameObject.name + ": HIT");
    }
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