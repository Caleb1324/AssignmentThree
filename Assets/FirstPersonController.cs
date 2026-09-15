using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float mouseSensitivity = 0.15f;
    public float gravity = -20f;

    private CharacterController controller;
    private Camera playerCamera;
    private float cameraPitch = 0f;
    private float verticalVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // WASD movement
        Vector2 movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 1;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 1;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 1;
        }

        Vector3 move = transform.right * movementInput.x +
                       transform.forward * movementInput.y;

        move = Vector3.ClampMagnitude(move, 1f);

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Mouse look
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSensitivity;
            float mouseY = mouseDelta.y * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

            playerCamera.transform.localRotation =
                Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        // Escape releases mouse
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}