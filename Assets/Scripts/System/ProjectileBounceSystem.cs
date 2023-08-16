using System.Linq;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileBounceSystem : ComponentSystem
{
    private EntityQuery _collisionQuery;
    private BounceAbility _bounceAbility;
    private Projectile _projectile;
    private RaycastHit _raycastHit;
    private Ray _ray;
    private Collider _collider;
    private float3 _reflectionDirection;

    protected override void OnCreate()
    {
        _collisionQuery = GetEntityQuery(
            ComponentType.ReadOnly<ProjectileObjectData>(),
            ComponentType.ReadOnly<ActorColliderData>(),
            ComponentType.ReadOnly<Transform>()
            );
    }

    protected override void OnUpdate()
    {
        Entities.With(_collisionQuery).ForEach(
                        (Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData) =>
                        {
                            ManageBounce(transform, ref projectileData, ref colliderData);
                        });

    }

    private void ManageBounce(Transform transform, ref ProjectileObjectData projectileData, ref ActorColliderData colliderData)
    {
        _bounceAbility = transform.gameObject.GetComponent<BounceAbility>();
        if (_bounceAbility == null) return;
        _bounceAbility.ActorColliderData = colliderData;
        _projectile = transform.gameObject.GetComponent<Projectile>();
        if (_projectile == null) return;
        if (!_projectile.ActiveProjectile) return;
        _collider = _bounceAbility.Collider;
        if (math.length(projectileData.Velocity) < 0.01f) return;
        _ray = new Ray(transform.position, projectileData.Velocity);
        bool hit = false;
        switch (colliderData.ColliderType)
        {
            case ColliderType.Sphere:
                hit = Physics.SphereCast(
                    transform.position,
                    colliderData.SphereRadius,
                    projectileData.Velocity,
                    out _raycastHit,
                    0.05f,
                    _bounceAbility.TargetMask);
                break;
            case ColliderType.Capsule:
                var point1 = colliderData.CapsuleStart + (float3)transform.position;
                var point2 = colliderData.CapsuleEnd + (float3)transform.position;
                var center = (point1 + point2) / 2f;
                point1 = (float3)(transform.rotation * (point1 - center)) + center;
                point2 = (float3)(transform.rotation * (point2 - center)) + center;
                hit = Physics.CapsuleCast(
                    point1,
                    point2,
                    colliderData.CapsuleRadius,
                    projectileData.Velocity,
                    out _raycastHit,
                    0.05f,
                    _bounceAbility.TargetMask);
                break;
            case ColliderType.Box:
                hit = Physics.BoxCast(
                    (float3)transform.position + colliderData.BoxCenter,
                    colliderData.BoxHalfExtents * 2f,
                    projectileData.Velocity,
                    out _raycastHit,
                    colliderData.BoxOrientation,
                    0.05f,
                    _bounceAbility.TargetMask);
                break;
            default:
                break;
        }
        if (hit)
        {
            _reflectionDirection = Vector3.Reflect(projectileData.Velocity, _raycastHit.normal);
            projectileData.ForwardDirectionSelf = transform.InverseTransformVector(math.normalize(_reflectionDirection));
        }
    }
}