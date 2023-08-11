using Unity.Entities;

public class CharacterDashSystem : ComponentSystem
{
    private EntityQuery _dashQuerry;
    private float _deltaTime;

    protected override void OnCreate()
    {
        _dashQuerry = GetEntityQuery(
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<DashData>(),
            ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        Entities.With(_dashQuerry).ForEach
            (
                (UserInputData userInputData, ref DashData dashData, ref InputData inputData) =>
                {
                    ManageDash(userInputData,  dashData, ref inputData, _deltaTime);
                }
            );
    }

    private void ManageDash(UserInputData userInputData, DashData dashData, ref InputData inputData, float deltaTime)
    {
        if (userInputData != null && userInputData.DashAction is IAbility ability)
        {
            if (inputData.DashDurationTimer > 0)
            {
                inputData.DashDurationTimer -= deltaTime;
            }
            else if (inputData.Dash > inputData.DeadZoneMagnitude)
            {
                inputData.DashDurationTimer = dashData.DashDuration;
                ability.Execute();
            }
        }
    }
}
