using TagsUtility;
using Unity.Mathematics;
using UnityEngine;

public class BounceAbility : CollisionAbility, ICollisionAbility
{
    public LayerMask TargetMask;
    public ActorColliderData ActorColliderData;
    public ProjectileObjectData projectileData;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public void Execute()
    {

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
    //    switch (ActorColliderData.ColliderType)
    //    {
    //        case ColliderType.Sphere:
    //            Gizmos.DrawSphere(
    //                (float3)transform.position + ActorColliderData.SphereCenter - math.normalize(projectileData.Velocity) * ActorColliderData.SphereRadius,
    //                ActorColliderData.SphereRadius);
    //            break;
    //        case ColliderType.Capsule:
    //            var point1 = ActorColliderData.CapsuleStart + (float3)transform.position;
    //            var point2 = ActorColliderData.CapsuleEnd + (float3)transform.position;
    //            var center = (point1 + point2) / 2f;
    //            point1 = (float3)(transform.rotation * (point1 - center)) + center;
    //            point2 = (float3)(transform.rotation * (point2 - center)) + center;
    //            Gizmos.DrawSphere(point1 - math.normalize(projectileData.Velocity) * math.length(point1 - point2), ActorColliderData.CapsuleRadius);
    //            Gizmos.DrawSphere(point2 - math.normalize(projectileData.Velocity) * math.length(point1 - point2), ActorColliderData.CapsuleRadius);
    //            break;
    //        case ColliderType.Box:
    //            Gizmos.DrawCube(
    //                (float3)transform.position + ActorColliderData.BoxCenter - math.normalize(projectileData.Velocity),
    //                ActorColliderData.BoxHalfExtents * 2f);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}