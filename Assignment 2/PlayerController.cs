using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;

    private InputAction interact;
    private InputAction move;
    Camera mainCamera;
    public float speed = 3f;

    private void Awake()
    {
        playerControls = new PlayerInputActions(); 
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        interact = playerControls.Player.Interaction;
        interact.Enable();
        interact.performed += Interaction;
    }

    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = move.ReadValue<Vector2>();
        Vector3 moveDirection = mainCamera.transform.forward * inputDirection.y + mainCamera.transform.right * inputDirection.x;
        moveDirection.y = 0;
        transform.position += moveDirection.normalized * speed * Time.deltaTime;
    }

    private void Interaction(InputAction.CallbackContext context)
    {
        Debug.Log("interaction?");
    }
}
