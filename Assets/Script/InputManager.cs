using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public InputActionReference moveAction;
    public InputActionReference crouchAction;
    public InputActionReference lookAction;
    public InputActionReference interactAction;


    private CharacterMovement motor;
    private PlayerLook look;

    public bool canMove = true;

    void Awake()
    {
        playerInput = new PlayerInput();

        motor = GetComponent<CharacterMovement>();
        look = GetComponent<PlayerLook>();

        moveAction.action.performed += ctx => motor.Crouch();
        crouchAction.action.performed += ctx => motor.Sprint();
        interactAction.action.performed += ctx => Interact();
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

    private void Interact()
    {

    }
}
