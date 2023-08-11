using Unity.Entities;
using Unity.Mathematics;

public class AnimatorParametersSetSystem : ComponentSystem
{
    private EntityQuery _animatorQuerry;

    protected override void OnCreate()
    {
        _animatorQuerry = GetEntityQuery(ComponentType.ReadOnly<UserInputData>(), ComponentType.ReadOnly<InputData>());
    }

    protected override void OnUpdate()
    {
        Entities.With(_animatorQuerry).ForEach
            (
                (UserInputData userInputData, ref InputData inputData) =>
                {
                    ManageAnimatorParameters(userInputData, ref inputData);
                }
            );
    }

    private void ManageAnimatorParameters(UserInputData userInputData, ref InputData inputData)
    {
        if (userInputData.animator != null)
        {
            userInputData.animator.SetFloat("MoveSpeed", math.length(inputData.Move));
        }
    }
}
