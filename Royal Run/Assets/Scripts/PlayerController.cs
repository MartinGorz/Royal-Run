using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Vector2 movement;
    Rigidbody rigidBody;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        
        Vector3 currentPosition = rigidBody.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);
        rigidBody.MovePosition(newPosition);
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        Debug.Log(movement);

    }
    
}
