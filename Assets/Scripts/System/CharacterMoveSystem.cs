using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMoveSystem : ComponentSystem
{
    private EntityQuery _movementQuery;
    private float3 _currentPosition;
    private float3 _direction;
    private float _deltaTime;
    private float _speedMultiplier;
    private float _directionMagnitude;

    protected override void OnCreate()
    {
        _movementQuery = GetEntityQuery(
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<MoveData>(),
            ComponentType.ReadOnly<Transform>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        Entities.With(_movementQuery).ForEach
            (
                (Transform transform, ref InputData inputData, ref MoveData moveData) =>
                {
                    ManageMovement(transform, ref inputData, ref moveData, _deltaTime);
                }
            );
    }

    private void ManageMovement(Transform transform, ref InputData inputData, ref MoveData moveData, float deltaTime)
    {
        _direction = new float3(inputData.Move.x, 0, inputData.Move.y);
        _directionMagnitude = (math.length(_direction));
        if ((_directionMagnitude < inputData.DeadZoneMagnitude) || (inputData.DashDurationTimer > 0))
        {
            return;
        }
        _currentPosition = transform.position;
        _speedMultiplier = deltaTime * moveData.Speed;
        _currentPosition += _direction* _speedMultiplier;
        transform.position = _currentPosition;
        transform.rotation = Quaternion.LookRotation(_direction);
    }
}
