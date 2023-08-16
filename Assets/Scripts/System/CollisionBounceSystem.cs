using System.Linq;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CollisionBounceSystem : ComponentSystem
{
    private EntityQuery _collisionQuery;
    private BounceAbility _bounceAbility;

    protected override void OnCreate()
    {
        _collisionQuery = GetEntityQuery(
            ComponentType.ReadOnly<BounceAbility>(),
            ComponentType.ReadOnly<RigidObjectData>(),
            ComponentType.ReadOnly<ActorColliderData>(),
            ComponentType.ReadOnly<Transform>()
            );
    }

    protected override void OnUpdate()
    {
        Entities.With(_collisionQuery).ForEach(
                        (Transform transform, ref ActorColliderData colliderData, ref RigidObjectData rigidObjectData) =>
                        {
                            ManageBounce(transform, ref rigidObjectData, ref colliderData);
                        });

    }

    private void ManageBounce(Transform transform, ref RigidObjectData rigidObjectData, ref ActorColliderData colliderData)
    {
        _bounceAbility = transform.gameObject.GetComponent<BounceAbility>();
        if (_bounceAbility == null)
        {
            return;
        }
        RaycastHit[] hit;
        switch (colliderData.ColliderType)
        {
            case ColliderType.Sphere:
                
            break;
            case ColliderType.Box:

            break;
            case ColliderType.Capsule:
            break;
            default: break;
        }
        // if (bounceAbility.Contacted)
        // {
        //     rigidObjectData.ForwardDirectionSelf = transform.InverseTransformVector(bounceAbility.ContactNormal);
        //     bounceAbility.Contacted = false;
        // }
    }
}