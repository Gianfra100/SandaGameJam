using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

[RequireComponent(typeof(Rigidbody2D))]
public class BasePlayer : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] protected float checkSphereRadius = 0.1f;
    [SerializeField] protected float checkUpDistance = .5f;
    [SerializeField] protected float checkDownDistance = .5f;
    [SerializeField] protected LayerMask groundLayers;

    Vector2 HitUpPoint => transform.position + Vector3.up * checkUpDistance;
    Vector2 HitDownPoint => transform.position + Vector3.down * checkDownDistance;

    [SerializeField] private bool grounded;

    private Rigidbody2D rigidBody;
    private float horizontalInput;

    private PlayerState state = PlayerState.Idle;
    private PlayerActionState actionState = PlayerActionState.None;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the player object.");
        }
    }

    void Update()
    {
        Move();
        CheckPlayerLand();
    }

    public virtual void Move()
    {
        float xVelocity = horizontalInput * moveSpeed;
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

    public virtual void Jump()
    {
        if (IsJumping()) return;

        if (IsGrounded())
        {
            actionState = PlayerActionState.Jumping;
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void SetHorizontalInput(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
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

        if (IsGrounded())
        {
            actionState = PlayerActionState.None;
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
