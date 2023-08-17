// using Unity.Entities;
// using Unity.Mathematics;
// using UnityEngine;

// public class ProjectileCollisionSystem : ComponentSystem
// {
//     private EntityQuery _collisionQuery;
//     private BounceAbility _bounceAbility;
//     private Projectile _projectile;
//     private RaycastHit _raycastHit;
//     private float3 _reflectionDirection;
//     private float3 _capsulePoint1;
//     private float3 _capsulePoint2;
//     private float3 _capsuleCenter;
//     private float3 _castBackupOffset;
//     private float _castDistance;

//     protected override void OnCreate()
//     {
//         _collisionQuery = GetEntityQuery(
//             ComponentType.ReadOnly<ProjectileObjectData>(),
//             ComponentType.ReadOnly<ActorColliderData>(),
//             ComponentType.ReadOnly<Transform>()
//             );
//     }

//     protected override void OnUpdate()
//     {
//         Entities.With(_collisionQuery).ForEach(
//                         (Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData) =>
//                         {
//                             ManageBounce(transform, ref projectileData, ref colliderData);
//                         });

//     }

//     private void ManageBounce(Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData)
//     {
//         _bounceAbility = transform.gameObject.GetComponent<BounceAbility>();
//         if (_bounceAbility == null) { Debug.Log("_bounceAbility == null"); return; }
//         _bounceAbility.ActorColliderData = colliderData;
//         _bounceAbility.projectileData = projectileData;
//         _projectile = transform.gameObject.GetComponent<Projectile>();
//         if (_projectile == null) { Debug.Log("_projectile == null"); return; }
//         if (!_projectile.ActiveProjectile) { Debug.Log("!_projectile.ActiveProjectile"); return; }
//         if (math.length(projectileData.Velocity) < 0.01f) { Debug.Log("LOW VELOCITY"); return; }
//         bool hit = false;
//         switch (colliderData.ColliderType)
//         {
//             case ColliderType.Sphere:
//                 _castBackupOffset = math.normalize(projectileData.Velocity) * colliderData.SphereRadius;
//                 _castDistance = math.length(_castBackupOffset);
//                 hit = Physics.SphereCast(
//                     (float3)transform.position - _castBackupOffset,
//                     colliderData.SphereRadius,
//                     projectileData.Velocity,
//                     out _raycastHit,
//                     _castDistance,
//                     _bounceAbility.TargetMask);
//                 break;
//             case ColliderType.Capsule:
//                 _capsulePoint1 = colliderData.CapsuleStart + (float3)transform.position;
//                 _capsulePoint2 = colliderData.CapsuleEnd + (float3)transform.position;
//                 _capsuleCenter = (_capsulePoint1 + _capsulePoint2) / 2f;
//                 _capsulePoint1 = (float3)(transform.rotation * (_capsulePoint1 - _capsuleCenter)) + _capsuleCenter;
//                 _capsulePoint2 = (float3)(transform.rotation * (_capsulePoint2 - _capsuleCenter)) + _capsuleCenter;
//                 _castBackupOffset = math.normalize(projectileData.Velocity) * math.length(_capsulePoint1 - _capsulePoint2);
//                 _castDistance = math.length(_castBackupOffset);
//                 hit = Physics.CapsuleCast(
//                     _capsulePoint1 - _castBackupOffset,
//                     _capsulePoint2 - _castBackupOffset,
//                     colliderData.CapsuleRadius,
//                     projectileData.Velocity,
//                     out _raycastHit,
//                     _castDistance,
//                     _bounceAbility.TargetMask);
//                 break;
//             // case ColliderType.Capsule:
//             //     var point1 = colliderData.CapsuleStart + (float3)transform.position;
//             //     var point2 = colliderData.CapsuleEnd + (float3)transform.position;
//             //     var center = (point1 + point2) / 2f;
//             //     point1 = (float3)(transform.rotation * (point1 - center)) + center;
//             //     point2 = (float3)(transform.rotation * (point2 - center)) + center;
//             //     _castBackupOffset = math.normalize(projectileData.Velocity) * math.length(point1 - point2);
//             //     _castDistance = math.length(_castBackupOffset);
//             //     hit = Physics.CapsuleCast(
//             //         point1 - _castBackupOffset,
//             //         point2 - _castBackupOffset,
//             //         colliderData.CapsuleRadius,
//             //         projectileData.Velocity,
//             //         out _raycastHit,
//             //         _castDistance,
//             //         _bounceAbility.TargetMask);
//             //     break;
//             case ColliderType.Box:
//                 _castBackupOffset = math.normalize(projectileData.Velocity) * colliderData.BoxHalfExtents * 2f;
//                 _castDistance = math.length(_castBackupOffset);
//                 hit = Physics.BoxCast(
//                     (float3)transform.position + colliderData.BoxCenter - _castBackupOffset,
//                     colliderData.BoxHalfExtents * 2f,
//                     projectileData.Velocity,
//                     out _raycastHit,
//                     colliderData.BoxOrientation,
//                     _castDistance,
//                     _bounceAbility.TargetMask);
//                 break;
//             default:
//                 break;
//         }
//         if (hit)
//         {
//             _reflectionDirection = Vector3.Reflect(projectileData.Velocity, _raycastHit.normal);
//             projectileData.ForwardDirectionSelf = transform.InverseTransformVector(math.normalize(_reflectionDirection));
//         }
//     }
// }