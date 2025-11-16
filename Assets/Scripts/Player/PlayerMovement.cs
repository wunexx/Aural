using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IUpdatable, IFixedUpdatable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float dashDuration = 0.25f;

    [Header("References")]
    [SerializeField] UpdateManager updateManager;

    Rigidbody2D rb;
    Vector2 input;

    float dashTimer = 0;
    float dashCooldownTimer = 0;

    bool isDashing = false;

    Vector2 dashDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        updateManager.AddUpdatable(this);
        updateManager.AddFixedUpdatable(this);
    }

    private void OnDisable()
    {
        updateManager.RemoveUpdatable(this);
        updateManager.RemoveFixedUpdatable(this);
    }

    public void OnUpdate()
    {
        input = PlayerInputController.Instance.GetMovementInput();

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (PlayerInputController.Instance.GetDashInput() == 1 && !isDashing && dashCooldownTimer <= 0f)
            StartDash();
    }

    public void OnFixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * dashSpeed * Time.fixedDeltaTime);

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
                isDashing = false;
        }
        else
            rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void StartDash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;
        dashTimer = dashDuration;

        dashDirection = input.sqrMagnitude > 0 ? input : transform.right;
    }
}