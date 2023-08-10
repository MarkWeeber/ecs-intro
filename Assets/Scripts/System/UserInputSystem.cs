using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputSystem : ComponentSystem
{
    private EntityQuery _movementQuery;
    private InputAction _moveAction;
    private float2 _moveInput;

    protected override void OnCreate()
    {
        _movementQuery = GetEntityQuery(ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnStartRunning()
    {
        _moveAction = new InputAction(name: "move", binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With(name: "Up", binding: "<Keyboard>/w")
            .With(name: "Down", binding: "<Keyboard>/s")
            .With(name: "Left", binding: "<Keyboard>/a")
            .With(name: "Down", binding: "<Keyboard>/d");
        _moveAction.performed += MoveActionPerformed;
        _moveAction.started += MoveActionPerformed;
        _moveAction.canceled += MoveActionPerformed;
        _moveAction.Enable();
    }

    private void MoveActionPerformed(InputAction.CallbackContext obj)
    {
        _moveInput = obj.ReadValue<Vector2>();
    }

    protected override void OnStopRunning()
    {
        _moveAction.Disable();
    }

    protected override void OnUpdate()
    {
        Entities.With(_movementQuery).ForEach(
                (UserInputData userInputData) =>
                {
                    MapUserInput(userInputData);
                }
            );
    }

    private void MapUserInput(UserInputData userInputData)
    {
        userInputData.Move = _moveInput;
    }

}
