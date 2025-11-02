using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public InputActionReference moveAction;
    public InputActionReference crouchAction;
    public InputActionReference lookAction;
    public InputActionReference escapeAction;


    private CharacterMovement motor;
    private PlayerLook look;
    public PlayerInteract interact;
    public PlayerInteract interactUsed;

    public bool canMove = true;

    void Awake()
    {
        playerInput = new PlayerInput();

        motor = GetComponent<CharacterMovement>();
        look = GetComponent<PlayerLook>();
        interact = FindFirstObjectByType<CinemachineStateDrivenCamera>().transform.GetChild(0).gameObject.GetComponent<PlayerInteract>();

        crouchAction.action.performed += ctx => motor.Crouch();
        // sprintAction.action.performed += ctx => motor.Sprint();
        escapeAction.action.performed += ctx => interact.QuitHole();
    }

    void FixedUpdate()
    {
        //tell the playmotor to move using the value from our movement action.
        if (canMove)
        {
            motor.ProcessMove(moveAction.action.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        look.ProcessLook(lookAction.action.ReadValue<Vector2>());
    }
}
