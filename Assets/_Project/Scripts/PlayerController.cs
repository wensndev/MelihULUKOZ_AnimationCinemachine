using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.5f;
    public float rotationSpeed = 12.0f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -20.0f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        var actions = InputSystem.actions;
        if (actions == null)
        {
            Debug.LogError(
                "InputSystem.actions is null.");
            return;
        }

        moveAction = actions.FindAction("Move", throwIfNotFound: false);
        jumpAction = actions.FindAction("Jump", throwIfNotFound: false);
        sprintAction = actions.FindAction("Sprint", throwIfNotFound: false);

        if (moveAction == null || jumpAction == null || sprintAction == null)
            Debug.LogError("Missing Actions.");
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        bool grounded = controller.isGrounded;

        Vector2 move2 = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
        Vector3 input = new Vector3(move2.x, 0f, move2.y).normalized;

        bool isRunning = sprintAction != null && sprintAction.IsPressed();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 horizontalDelta = Vector3.zero;

        if (input.magnitude >= 0.1f)
        {
            Vector3 moveDirection = input;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            horizontalDelta = moveDirection * currentSpeed * Time.deltaTime;
        }

        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        if (jumpAction != null && jumpAction.WasPressedThisFrame() && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 verticalDelta = new Vector3(0f, velocity.y * Time.deltaTime, 0f);
        controller.Move(horizontalDelta + verticalDelta);

        float animationSpeed = 0f;

        if (input.magnitude > 0.1f)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("Speed", animationSpeed);
    }
}
