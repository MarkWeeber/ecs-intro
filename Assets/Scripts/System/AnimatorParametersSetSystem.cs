using Unity.Entities;
using Unity.Mathematics;

public class AnimatorParametersSetSystem : ComponentSystem
{
    private EntityQuery _animatorQuery;

    protected override void OnCreate()
    {
        _animatorQuery = GetEntityQuery(ComponentType.ReadOnly<UserInputData>(), ComponentType.ReadOnly<InputData>());
    }

    protected override void OnUpdate()
    {
        Entities.With(_animatorQuery).ForEach
            (
                (UserInputData userInputData, ref InputData inputData) =>
                {
                    ManageAnimatorParameters(userInputData, ref inputData);
                }
            );
    }

    private void ManageAnimatorParameters(UserInputData userInputData, ref InputData inputData)
    {
        if (userInputData.Animator != null)
        {
            userInputData.Animator.SetFloat("MoveSpeed", math.length(inputData.Move));
        }
    }
}
