using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class BasePlayer : MonoBehaviour, IGetHit
{
    [Header("Player Properties")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float airSpeed = 2.5f;
    [SerializeField] protected float jumpHeight = 7f;
    [SerializeField] protected float jumpTimePeak = 0.5f;
    [SerializeField] protected float jumpTimeDescend = 0.5f;

    [Header("Ground Check")]
    [SerializeField] protected float checkSphereRadius = 0.1f;
    [SerializeField] protected float checkUpDistance = .5f;
    [SerializeField] protected float checkDownDistance = .5f;
    [SerializeField] protected LayerMask groundLayers;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text actionStateText;

    Vector2 HitUpPoint => transform.position + Vector3.up * checkUpDistance;
    Vector2 HitDownPoint => transform.position + Vector3.down * checkDownDistance;

    [SerializeField] private bool grounded;

    private Rigidbody2D rigidBody;
    private PlayerState state = PlayerState.Idle;
    private PlayerActionState actionState = PlayerActionState.None;
    private float horizontalInput;
    private float jumpVelocity;
    private float jumpGravity;
    private float jumpDescentGravity;
    public Charge playerCharge;
    private MagneticEntity magneticEntity;
    private void OnEnable()
    {
        GameEvents.OnGameLose += StopPlayer;
        GameEvents.OnGameWin += StopPlayer;

        PlayerAnimationsEvents.OnPlayerDeathEnd += PlayerDeathEnd;
    }

    private void OnDisable()
    {
        GameEvents.OnGameLose -= StopPlayer;
        GameEvents.OnGameWin -= StopPlayer;

        PlayerAnimationsEvents.OnPlayerDeathEnd -= PlayerDeathEnd;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the player object.");
        }

        magneticEntity = GetComponent<MagneticPlayer>();

        if (magneticEntity != null) playerCharge = magneticEntity.charge;

        jumpVelocity = 2f * jumpHeight / jumpTimePeak;
        jumpGravity = 2f * jumpHeight / (jumpTimePeak * jumpTimePeak);
        jumpDescentGravity = 2f * jumpHeight / (jumpTimeDescend * jumpTimeDescend);
    }

    void FixedUpdate()
    {
        if (IsDead() || IsStopped()) return;

        Move();
        Jumping();
        CheckPlayerLand();

        if (debugMode) DebugMode();
    }

    public void SetHorizontalInput(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    public void PerformJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartJump();
        }

        if (context.canceled)
        {
            CutJump();
        }
    }


    protected virtual void Move()
    {
        float speed = IsJumping() ? airSpeed : moveSpeed;
        float xVelocity = horizontalInput * speed;
        if (xVelocity != 0)
        {
            state = PlayerState.Moving;
        }
        else
        {
            xVelocity = 0;
            state = PlayerState.Idle;
        }
        rigidBody.linearVelocity = new Vector2(xVelocity, rigidBody.linearVelocity.y);
    }


    protected virtual void StartJump()
    {
        if (IsJumping()) return;

        if (IsGrounded())
        {
            actionState = PlayerActionState.Jump;
            float finalJumpVelocity = jumpVelocity;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, finalJumpVelocity);
            rigidBody.gravityScale = 0;
        }
    }

    protected virtual void Jumping()
    {
        if (IsJumping())
        {

            float velocityY = rigidBody.linearVelocity.y;
            velocityY += (velocityY > 0 ? jumpGravity : jumpDescentGravity) * Time.fixedDeltaTime;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, velocityY);
        }
    }

    protected virtual void CutJump()
    {
        if (IsJumping() && rigidBody.linearVelocity.y > 0)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.y * 0.5f);
        }
    }

    private bool IsJumping() { return actionState == PlayerActionState.Jump; }

    private bool IsGrounded()
    {
        RaycastHit2D hitUp = Physics2D.CircleCast(HitUpPoint, checkSphereRadius, Vector2.up, 0f, groundLayers);
        RaycastHit2D hitDown = Physics2D.CircleCast(HitDownPoint, checkSphereRadius, Vector2.down, 0f, groundLayers);
        grounded = hitUp.collider != null || hitDown.collider != null;
        return grounded;
    }

    private void CheckPlayerLand()
    {
        if (!IsJumping()) return;

        if (IsGrounded() && rigidBody.linearVelocity.y <= 0)
        {
            actionState = PlayerActionState.None;
            rigidBody.gravityScale = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, HitUpPoint);
        Gizmos.DrawSphere(HitUpPoint, checkSphereRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, HitDownPoint);
        Gizmos.DrawSphere(HitDownPoint, checkSphereRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEvents.GameWin();
        }
    }

    public void GetHit()
    {
        Dead();
    }

    private void Dead()
    {
        state = PlayerState.Dead;
        rigidBody.linearVelocity = Vector2.zero;
        StopPlayer();
    }

    private void PlayerDeathEnd()
    {
        GameEvents.GameLose();
    }

    private void StopPlayer()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.gravityScale = 0;
        GetComponent<PlayerInput>().enabled = false;
        if (state == PlayerState.Dead) return;
        state = PlayerState.None;
    }

    private bool IsDead() { return state == PlayerState.Dead; }
    private bool IsStopped() { return state == PlayerState.None; }

    public PlayerState GetPlayerState() { return state; }
    public Vector2 GetVelocity()
    {
        if(rigidBody == null) return Vector2.zero;
        
        return rigidBody.linearVelocity;
    }
    

    private void DebugMode()
    {
        stateText.text = $"State: {state}";
        actionStateText.text = $"Action State: {actionState}";
    }
}
