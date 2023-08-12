using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputSystem : ComponentSystem
{
    private EntityQuery _userInputQuerry;
    private InputAction _moveAction;
    private InputAction _dashAction;
    private InputAction _fireAction;
    private float2 _moveInput;
    private float _dashInput;
    private float _fireInput;

    protected override void OnCreate()
    {
        _userInputQuerry = GetEntityQuery(ComponentType.ReadOnly<InputData>());
    }

    protected override void OnStartRunning()
    {
        _moveAction = new InputAction(name: "move", binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With(name: "Up", binding: "<Keyboard>/w")
            .With(name: "Down", binding: "<Keyboard>/s")
            .With(name: "Left", binding: "<Keyboard>/a")
            .With(name: "Right", binding: "<Keyboard>/d");
        _moveAction.performed += MoveActionPerformed;
        _moveAction.started += MoveActionPerformed;
        _moveAction.canceled += MoveActionPerformed;
        _moveAction.Enable();

        _dashAction = new InputAction(name: "dash", binding: "<Keyboard>/Leftshift");
        _dashAction.performed += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.canceled += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.Enable();

        _fireAction = new InputAction(name: "fire", binding: "<Keyboard>/Space");
        _fireAction.performed += context => { _fireInput = context.ReadValue<float>(); };
        _fireAction.canceled += context => { _fireInput = context.ReadValue<float>(); };
        _fireAction.Enable();
    }

    private void MoveActionPerformed(InputAction.CallbackContext obj)
    {
        _moveInput = obj.ReadValue<Vector2>();
    }

    protected override void OnStopRunning()
    {
        _moveAction.Disable();
        _dashAction.Disable();
        _fireAction.Disable();
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
