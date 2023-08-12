using System.Linq;
using Unity.Entities;

public class CharacterFireSystem : ComponentSystem
{
    private EntityQuery _fireQuerry;
    private float _deltaTime;
    private FireAbility _fireAbility;

    protected override void OnCreate()
    {
        _fireQuerry = GetEntityQuery(
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<FireData>(),
            ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnUpdate()
    {
        _deltaTime = Time.DeltaTime;
        Entities.With(_fireQuerry).ForEach
            (
                (UserInputData userInputData, ref FireData fireData, ref InputData inputData) =>
                {
                    ManageFire(userInputData, ref fireData, ref inputData, _deltaTime);
                }
            );
    }

    private void ManageFire(UserInputData userInputData, ref FireData fireData, ref InputData inputData, float deltaTime)
    {
        if (userInputData.Abilities.Count > 0)
        {
            _fireAbility = userInputData.Abilities.OfType<FireAbility>().First();
            if (_fireAbility != null)
            {
                if (fireData.Engaged)
                {
                    if (inputData.BusyByAbilityTimer > 0)
                    {
                        inputData.BusyByAbilityTimer -= deltaTime;
                    }
                    else
                    {
                        fireData.Engaged = false;
                    }
                }
                else
                {
                    if (inputData.BusyByAbilityTimer <= 0)
                    {
                        if (inputData.Fire > inputData.DeadZoneMagnitude)
                        {
                            inputData.BusyByAbilityTimer = _fireAbility.Duration;
                            fireData.Engaged = true;
                            _fireAbility.Execute();
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
