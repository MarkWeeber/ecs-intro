using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputSystem : ComponentSystem
{
    private CharacterControlsScheme charControlScheme;
    private EntityQuery _userInputQuerry;
    private float2 _moveInput;
    private float _dashInput;
    private float _fireInput;

    protected override void OnCreate()
    {
        charControlScheme = new CharacterControlsScheme();
        _userInputQuerry = GetEntityQuery(ComponentType.ReadOnly<InputData>());
    }

    protected override void OnStartRunning()
    {
        charControlScheme.Character.Move.performed += MoveActionPerformed;
        charControlScheme.Character.Move.started += MoveActionPerformed;
        charControlScheme.Character.Move.canceled += MoveActionPerformed;
        charControlScheme.Character.Move.Enable();

        charControlScheme.Character.Dash.performed += context => { _dashInput = context.ReadValue<float>(); };;
        charControlScheme.Character.Dash.canceled += context => { _dashInput = context.ReadValue<float>(); };;
        charControlScheme.Character.Dash.Enable();

        charControlScheme.Character.Fire.performed += context => { _fireInput = context.ReadValue<float>(); };;
        charControlScheme.Character.Fire.canceled += context => { _fireInput = context.ReadValue<float>(); };;
        charControlScheme.Character.Fire.Enable();

    }

    private void MoveActionPerformed(InputAction.CallbackContext obj)
    {
        _moveInput = obj.ReadValue<Vector2>();
    }

    protected override void OnStopRunning()
    {
        charControlScheme.Character.Move.Enable();
        charControlScheme.Character.Dash.Enable();
        charControlScheme.Character.Fire.Enable();
    }

    protected override void OnUpdate()
    {
        Entities.With(_userInputQuerry).ForEach(
                (ref InputData inputData) =>
                {
                    MapUserInput(ref inputData);
                }
            );
    }

    private void MapUserInput(ref InputData inputData)
    {
        inputData.Move = _moveInput;
        inputData.Dash = _dashInput;
        inputData.Fire = _fireInput;
    }
}
