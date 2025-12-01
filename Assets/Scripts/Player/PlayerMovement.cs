using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : PlayerComponentBase, IUpdatable, IFixedUpdatable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float dashDuration = 0.25f;

    [Header("References")]
    [SerializeField] UpdateManager updateManager;
    [SerializeField] TrailRenderer dashTrail;
    [SerializeField] ParticleSystem footstepsParticles;

    [Header("Audio")]
    [SerializeField] AudioClip dashSound;
    [SerializeField] float volume = 0.2f;

    Animator animator;
    Rigidbody2D rb;
    Vector2 input;

    float dashTimer = 0;
    float dashCooldownTimer = 0;

    bool isDashing = false;

    Vector2 dashDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        dashTrail.emitting = false;
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

        animator.SetFloat("movement", input.magnitude);

        if (input.magnitude > 0.001f)
        {
            if (!footstepsParticles.isPlaying)
                footstepsParticles.Play();
        }
        else
        {
            if (footstepsParticles.isPlaying)
                footstepsParticles.Stop();
        }


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
            {
                isDashing = false;
                dashTrail.emitting = false;
            }
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

        dashTrail.emitting = true;

        SoundManager.Instance.PlayOtherSFX(dashSound, volume);
    }

    public override void OnPlayerDeath()
    {
        rb.linearVelocity = Vector2.zero;

        base.OnPlayerDeath();
    }
}