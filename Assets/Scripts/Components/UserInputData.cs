using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    public HashSet<MonoBehaviour> Abilities = new HashSet<MonoBehaviour>();
    [HideInInspector]
    public Animator Animator;
    [SerializeField]
    private float movementSpeed = 1f;
    [SerializeField]
    private float deadZoneMagnitude = 0.1f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<InputData>(entity, new InputData {DeadZoneMagnitude = deadZoneMagnitude, BusyByAbilityTimer = 0});
        dstManager.AddComponentData<MoveData>(entity, new MoveData {Speed = movementSpeed});
        if (Abilities.Count > 0)
        {
            foreach (MonoBehaviour item in Abilities)
            {
                if (item is IAbility)
                {
                    switch (item)
                    {
                        case FireAbility fireAbility:
                            dstManager.AddComponentData<FireData>(entity, new FireData());;
                            break;
                        case DashAbility dashAbility:
                            dstManager.AddComponentData<DashData>(entity, new DashData());
                            break;
                        default:
                        break;
                    }
                }
            }
        }
    }
}

public struct InputData : IComponentData
{
    public float2 Move;
    public float DeadZoneMagnitude;
    public float Dash;
    public float BusyByAbilityTimer;
    public float Fire;
}

public struct MoveData : IComponentData
{
    public float Speed;
}

public struct DashData : IComponentData
{
    public float DashDuration;
    public bool Engaged;
    public DashData(float DashDuration = 0, bool Engaged = false)
    {
        this.DashDuration = DashDuration;
        this.Engaged = Engaged;
    }
}

public struct FireData : IComponentData
{
    public float ProjectileSpeed;
    public bool Engaged;
    public FireData(float ProjectileSpeed = 0, bool Engaged = false)
    {
        this.ProjectileSpeed = ProjectileSpeed;
        this.Engaged = Engaged;
    }
}