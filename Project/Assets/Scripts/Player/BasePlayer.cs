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

    Vector2 HitUpPoint => transform.position + transform.up * checkUpDistance;
    Vector2 HitDownPoint => transform.position + -transform.up * checkDownDistance;

    [SerializeField] private bool grounded;

    private Rigidbody2D rigidBody;
    private PlayerState state = PlayerState.Idle;
    private PlayerActionState actionState = PlayerActionState.None;
    private float horizontalInput;
    private float jumpVelocity;
    private float jumpGravity;
    private float jumpDescentGravity;
    private MagneticEntity magneticEntity;
    public Charge playerCharge;
    public bool isRepelFromSurface = false;

    public Platforms currentPlatform;
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

        if (magneticEntity != null) playerCharge = magneticEntity.GetCharge();
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
            state = PlayerState.Idle;
        }

        Vector2 finalVelocity = new Vector2(xVelocity, rigidBody.linearVelocity.y);

        // 👇 sumamos la velocidad de la plataforma si está grounded en ella
        if (IsGrounded() && currentPlatform != null)
        {
            finalVelocity += currentPlatform.PlatformVelocity;
        }

        rigidBody.linearVelocity = finalVelocity;
    }


    protected virtual void StartJump()
    {
        if (IsJumping()) return;

        if (IsGrounded())
        {
            SetJumpValues();
            actionState = PlayerActionState.Jump;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpVelocity);
            rigidBody.gravityScale = 0;
            AudioManager.Instance?.Play2DSound(PlayerSoundsEnum.Jump);
        }
    }

    protected virtual void Jumping()
    {
        if (IsJumping() && !isRepelFromSurface)
        {
            float velocityY = rigidBody.linearVelocity.y;

            if (GetGravityDirection() < 0)
            {
                velocityY += (velocityY > 0 ? jumpGravity : jumpDescentGravity) * Time.fixedDeltaTime;
            }
            else if (GetGravityDirection() > 0)
            {
                velocityY += (velocityY < 0 ? jumpGravity : jumpDescentGravity) * Time.fixedDeltaTime;
            }

            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, velocityY);
        }
    }

    private void SetJumpValues()
    {
        jumpVelocity = transform.up.y * 2f * jumpHeight / jumpTimePeak;
        jumpGravity = GetGravityDirection() * 2f * jumpHeight / (jumpTimePeak * jumpTimePeak);
        jumpDescentGravity = GetGravityDirection() * 2f * jumpHeight / (jumpTimeDescend * jumpTimeDescend);
    }

    protected virtual void CutJump()
    {
        bool isPlayerAscend = GetGravityDirection() < 0 ? rigidBody.linearVelocity.y > 0 : rigidBody.linearVelocity.y < 0;

        if (IsJumping() && isPlayerAscend)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.y * 0.5f);
        }
    }

    public void CutJumpFromRepel()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.gravityScale = GetGravityDirection() < 0 ? 1 : -1;
        isRepelFromSurface = true;
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
        if (!IsJumping() && !isRepelFromSurface) return;

        bool isPlayerFalled = GetGravityDirection() < 0 ? rigidBody.linearVelocity.y <= 0 : rigidBody.linearVelocity.y >= 0;

        if (IsGrounded() && isPlayerFalled)
        {
            isRepelFromSurface = false;
            actionState = PlayerActionState.None;
            rigidBody.gravityScale = GetGravityDirection() < 0 ? 1 : -1;
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

        if (collision.gameObject.TryGetComponent(out Platforms platform))
        {
            currentPlatform = platform;
            platform.GetComponent<MagneticSurface>().OnChargeChange += RepelFromPlatfomr;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Platforms platform) && platform == currentPlatform)
        {
            currentPlatform.GetComponent<MagneticSurface>().OnChargeChange -= RepelFromPlatfomr;
            currentPlatform = null;
        }
    }

    public void GetHit()
    {
        Dead();
    }

    private void Dead()
    {
        AudioManager.Instance?.Play2DSound(PlayerSoundsEnum.Death);
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
        if (rigidBody == null) return Vector2.zero;

        return rigidBody.linearVelocity;
    }


    private void DebugMode()
    {
        stateText.text = $"State: {state}";
        actionStateText.text = $"Action State: {actionState}";
    }

    public void AttractToSurface(float direction)
    {
        AudioManager.Instance.Play2DSound(PlayerSoundsEnum.Attract);

        if (direction > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            rigidBody.gravityScale = -1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rigidBody.gravityScale = 1;
        }
    }

    private int GetGravityDirection()
    {
        return (int)-transform.up.y;
    }

    public void RepelFromSurface(Vector2 direction, float RepelForce)
    {
        AudioManager.Instance.Play2DSound(PlayerSoundsEnum.Repel);
        CutJumpFromRepel();
        rigidBody.AddForce(direction * RepelForce, ForceMode2D.Impulse);
    }

    public void RepelFromPlatfomr()
    {
        AudioManager.Instance.Play2DSound(PlayerSoundsEnum.Repel);
        CutJumpFromRepel();
        Vector2 dir =  (transform.position - currentPlatform.transform.position).normalized;
        rigidBody.AddForce(dir * currentPlatform.GetComponent<MagneticSurface>().force, ForceMode2D.Impulse);
    }
}
