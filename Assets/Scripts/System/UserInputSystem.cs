using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static CharacterControlsScheme;

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
        charControlScheme.Character.Move.performed += HandleMove;
        charControlScheme.Character.Move.performed += context => {_moveInput = context.ReadValue<Vector2>(); };
        charControlScheme.Character.Move.started += context => { _moveInput = context.ReadValue<Vector2>(); };
        charControlScheme.Character.Move.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
        charControlScheme.Character.Dash.performed += context => { _dashInput = context.ReadValue<float>(); };
        charControlScheme.Character.Dash.canceled += context => { _dashInput = context.ReadValue<float>(); };
        charControlScheme.Character.Fire.performed += context => { _fireInput = context.ReadValue<float>(); };
        charControlScheme.Character.Fire.canceled += context => { _fireInput = context.ReadValue<float>(); };
        charControlScheme.Character.Enable();

    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    protected override void OnStopRunning()
    {
        charControlScheme.Character.Disable();
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

    private void MoveActionPerformed(InputAction.CallbackContext obj)
    {
        _moveInput = obj.ReadValue<Vector2>();
    }
}
