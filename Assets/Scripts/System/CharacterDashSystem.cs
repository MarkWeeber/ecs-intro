using System.Linq;
using Unity.Entities;
using UnityEngine;

public class CharacterDashSystem : ComponentSystem
{
    private EntityQuery _dashQuerry;
    private float _deltaTime;
    private DashAbility _dashAbility;

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
                    ManageDash(userInputData, ref dashData, ref inputData, _deltaTime);
                }
            );
    }

    private void ManageDash(UserInputData userInputData, ref DashData dashData, ref InputData inputData, float deltaTime)
    {
        if (userInputData.Abilities.Count > 0)
        {
            _dashAbility = userInputData.Abilities.OfType<DashAbility>().First();
            if (_dashAbility != null)
            {
                if (dashData.Engaged)
                {
                    if (inputData.BusyByAbilityTimer > 0)
                    {
                        inputData.BusyByAbilityTimer -= deltaTime;
                    }
                    else
                    {
                        dashData.Engaged = false;
                    }
                }
                else
                {
                    if (inputData.BusyByAbilityTimer <= 0)
                    {
                        if (inputData.Dash > inputData.DeadZoneMagnitude)
                        {
                            inputData.BusyByAbilityTimer = _dashAbility.Duration;
                            dashData.Engaged = true;
                            _dashAbility.Execute();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
