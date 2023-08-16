using TagsUtility;
using Unity.Mathematics;
using UnityEngine;

public class BounceAbility : CollisionAbility, ICollisionAbility
{
    public LayerMask targetMask;
    public float3 ContactNormal;
    public bool Contacted;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Contacted = false;
    }

    public void Execute()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        ContactNormal = other.contacts[0].normal;
        Contacted = true;
    }
}