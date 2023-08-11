using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    public MonoBehaviour DashAction;
    public Animator animator;
    [SerializeField]
    private float movementSpeed = 1f;
    [SerializeField]
    private float deadZoneMagnitude = 0.1f;
    [SerializeField]
    private float dashDuration = 1f;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<InputData>(entity, new InputData {DeadZoneMagnitude = deadZoneMagnitude, DashDurationTimer = 0});
        dstManager.AddComponentData<MoveData>(entity, new MoveData {Speed = movementSpeed});
        if (DashAction != null && DashAction is IAbility)
        {
            dstManager.AddComponentData<DashData>(entity, new DashData { DashDuration = dashDuration });
        }
    }
}

public struct InputData : IComponentData
{
    public float2 Move;
    public float DeadZoneMagnitude;
    public float Dash;
    public float DashDurationTimer;
}

public struct MoveData : IComponentData
{
    public float Speed;
}

public struct DashData : IComponentData
{
    public float DashDuration;
}