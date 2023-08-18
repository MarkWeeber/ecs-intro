using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CollisionSystem : ComponentSystem
{
    private EntityQuery _collisionQuery;
    private EntityQuery _collisionWithBounceQuery;
    private Collider[] _results = new Collider[50];
    private ICollisionAbility _collisionAbility;
    private BounceAbility _bounceAbility;
    private Projectile _projectile;
    private RaycastHit _raycastHit;
    private float3 _reflectionDirection;
    private float3 _position;
    private float3 _capsulePoint1;
    private float3 _capsulePoint2;
    private float3 _capsuleCenter;
    private float3 _castBackupOffset;
    private Quaternion _rotation;
    private float _castDistance;
    private int _collidersCount;

    protected override void OnCreate()
    {
        _collisionQuery = GetEntityQuery(
            ComponentType.ReadOnly<ActorColliderData>(),
            ComponentType.ReadOnly<Transform>()
            );
        _collisionWithBounceQuery = GetEntityQuery(
            ComponentType.ReadOnly<ProjectileObjectData>(),
            ComponentType.ReadOnly<ActorColliderData>(),
            ComponentType.ReadOnly<Transform>()
            );
    }

    protected override void OnUpdate()
    {
        Entities.With(_collisionQuery).ForEach(
                (Entity entity, Transform transform, ref ActorColliderData colliderData) =>
                {
                    ManageCollisionWithOther(transform, ref colliderData);
                    {
                        // var gameObject = transform.gameObject;
                        // ICollisionAbility collisionAbility = gameObject.GetComponent<ICollisionAbility>();
                        // float3 position = gameObject.transform.position;
                        // Quaternion rotation = gameObject.transform.rotation;

                        // if (collisionAbility == null)
                        // {
                        //     return;
                        // }

                        // collisionAbility.Collisions?.Clear();

                        // int size = 0;

                        // switch (colliderData.ColliderType)
                        // {
                        //     case ColliderType.Sphere:
                        //         size = Physics.OverlapSphereNonAlloc(colliderData.SphereCenter + position, colliderData.SphereRadius, _results);
                        //         break;
                        //     case ColliderType.Capsule:
                        //         var point1 = colliderData.CapsuleStart + position;
                        //         var point2 = colliderData.CapsuleEnd + position;
                        //         var center = (point1 + point2) / 2f;
                        //         point1 = (float3)(rotation * (point1 - center)) + center;
                        //         point2 = (float3)(rotation * (point2 - center)) + center;
                        //         size = Physics.OverlapCapsuleNonAlloc(point1, point2, colliderData.CapsuleRadius, _results);
                        //         break;
                        //     case ColliderType.Box:
                        //         size = Physics.OverlapBoxNonAlloc(colliderData.BoxCenter + position, colliderData.BoxHalfExtents, _results, colliderData.BoxOrientation * rotation);
                        //         break;
                        //     default:
                        //         break;
                        // }

                        // if (size > 0)
                        // {
                        //     collisionAbility.Collisions = _results.ToList();
                        //     collisionAbility.Execute();
                        // }
                    }
                }
            );
        Entities.With(_collisionWithBounceQuery).ForEach(
                (Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData) =>
                {
                    ManageProjectileBounce(transform, ref projectileData, ref colliderData);
                }
        );
    }

    private void ManageCollisionWithOther(Transform transform, ref ActorColliderData colliderData)
    {
        _collisionAbility = transform.gameObject.GetComponent<ICollisionAbility>();
        if (_collisionAbility == null)
        {
            return;
        }
        CollisionAbility __collisionAbility = transform.gameObject.GetComponent<CollisionAbility>();
        if(__collisionAbility != null)
        {
            __collisionAbility._ActorColliderData = colliderData;
        }
        _position = transform.position;
        _rotation = transform.rotation;
        _collisionAbility.Collisions?.Clear();
        _collidersCount = 0;
        switch (colliderData.ColliderType)
        {
            case ColliderType.Sphere:
                _collidersCount = Physics.OverlapSphereNonAlloc(colliderData.SphereCenter + _position, colliderData.SphereRadius, _results);
                break;
            case ColliderType.Capsule:
                _capsulePoint1 = colliderData.CapsuleStart + _position;
                _capsulePoint2 = colliderData.CapsuleEnd + _position;
                _capsuleCenter = (_capsulePoint1 + _capsulePoint2) / 2f;
                _capsulePoint1 = (float3)(_rotation * (_capsulePoint1 - _capsuleCenter)) + _capsuleCenter;
                _capsulePoint2 = (float3)(_rotation * (_capsulePoint2 - _capsuleCenter)) + _capsuleCenter;
                _collidersCount = Physics.OverlapCapsuleNonAlloc(_capsulePoint1, _capsulePoint2, colliderData.CapsuleRadius, _results);
                break;
            case ColliderType.Box:
                _collidersCount = Physics.OverlapBoxNonAlloc(colliderData.BoxCenter + _position, colliderData.BoxHalfExtents, _results, colliderData.BoxOrientation * _rotation);
                break;
            default:
                break;
        }
        if (_collidersCount > 0)
        {
            _collisionAbility.Collisions = _results.ToList();
            _collisionAbility.Execute();
        }
    }

    private void ManageProjectileBounce(Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData)
    {
        SelfDestructAbility sda = transform.GetComponent<SelfDestructAbility>();
        if (sda != null)
        {
            sda.ActorColliderData = colliderData;
            sda.projectileData = projectileData;
        }
        _bounceAbility = transform.gameObject.GetComponent<BounceAbility>();
        if (_bounceAbility == null)
        {
            return;
        }
        _bounceAbility.ActorColliderData = colliderData;
        _bounceAbility.projectileData = projectileData;
        _projectile = transform.gameObject.GetComponent<Projectile>();
        if (_projectile == null)
        {
            return;
        }
        if (!_projectile.ActiveProjectile)
        {
            return;
        }
        if (math.length(projectileData.Velocity) < 0.01f)
        {
            return;
        }
        bool hit = false;
        _position = transform.position;
        _rotation = transform.rotation;
        switch (colliderData.ColliderType)
        {
            case ColliderType.Sphere:
                _castBackupOffset = math.normalize(projectileData.Velocity) * colliderData.SphereRadius;
                _castDistance = math.length(_castBackupOffset);
                hit = Physics.SphereCast(
                    (float3)transform.position,
                    colliderData.SphereRadius,
                    projectileData.Velocity,
                    out _raycastHit,
                    _castDistance * 0.5f,
                    _bounceAbility.TargetMask);
                break;
            case ColliderType.Capsule:
                _capsulePoint1 = colliderData.CapsuleStart + _position;
                _capsulePoint2 = colliderData.CapsuleEnd + _position;
                _capsuleCenter = (_capsulePoint1 + _capsulePoint2) / 2f;
                _capsulePoint1 = (float3)(_rotation * (_capsulePoint1 - _capsuleCenter)) + _capsuleCenter;
                _capsulePoint2 = (float3)(_rotation * (_capsulePoint2 - _capsuleCenter)) + _capsuleCenter;
                _castBackupOffset = math.normalize(projectileData.Velocity) * math.length(_capsulePoint1 - _capsulePoint2);
                _castDistance = math.length(_castBackupOffset);
                hit = Physics.CapsuleCast(
                    _capsulePoint1,
                    _capsulePoint2,
                    colliderData.CapsuleRadius,
                    projectileData.Velocity,
                    out _raycastHit,
                    _castDistance * 0.5f,
                    _bounceAbility.TargetMask);
                break;
            case ColliderType.Box:
                _castBackupOffset = math.normalize(projectileData.Velocity) * colliderData.BoxHalfExtents * 2f;
                _castDistance = math.length(_castBackupOffset);
                hit = Physics.BoxCast(
                    (float3)transform.position + colliderData.BoxCenter,
                    colliderData.BoxHalfExtents,
                    projectileData.Velocity,
                    out _raycastHit,
                    colliderData.BoxOrientation * transform.rotation,
                    _castDistance * 0.5f,
                    _bounceAbility.TargetMask);
                break;
            default:
                break;
        }
        if (hit)
        {
            //if(_raycastHit.collider.transform.gameObject.Equals(transform.gameObject))
            //{
            //    return;
            //}
            _reflectionDirection = Vector3.Reflect(projectileData.Velocity, _raycastHit.normal);
            projectileData.ForwardDirectionSelf = transform.InverseTransformVector(math.normalize(_reflectionDirection));
        }
    }
}