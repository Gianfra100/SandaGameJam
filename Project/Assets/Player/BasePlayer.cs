using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BasePlayer : MonoBehaviour
{
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


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the player object.");
        }

        jumpVelocity = 2f * jumpHeight / jumpTimePeak;
        jumpGravity = -2f * jumpHeight / (jumpTimePeak * jumpTimePeak);
        jumpDescentGravity = -2f * jumpHeight / (jumpTimeDescend * jumpTimeDescend);
    }

    void FixedUpdate()
    {
        Move();
        Jumping();
        CheckPlayerLand();
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
            actionState = PlayerActionState.Jumping;
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpVelocity);
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

    private bool IsJumping() { return actionState == PlayerActionState.Jumping; }

    private bool IsGrounded()
    {
        RaycastHit2D hitUp = Physics2D.CircleCast(HitUpPoint, checkSphereRadius, Vector2.up, 0f, groundLayers);
        RaycastHit2D hitDown = Physics2D.CircleCast(HitDownPoint, checkSphereRadius, Vector2.down, 0f, groundLayers);
        return hitUp.collider != null || hitDown.collider != null;
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
}
