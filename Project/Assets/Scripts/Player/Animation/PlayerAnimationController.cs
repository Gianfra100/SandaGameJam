using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private BasePlayer player;

    private Animator animator;

    private SpriteRenderer playerSpriteRenderer;

    PlayerState currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the player sprite object.");
        }

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing from the player sprite object.");
        }

        if (player == null)
        {
            player = GetComponentInParent<BasePlayer>();
            if (player == null)
            {
                Debug.LogError("BasePlayer reference is missing in PlayerAnimationController.");
            }
        }
    }
    void Update()
    {
        if (player == null || animator == null || playerSpriteRenderer == null) return;

        SetPlayerOrientation();
        PlayPlayerAnimation(player.GetPlayerState());
    }

    private void PlayPlayerAnimation(PlayerState state)
    {
        if (currentState == state) return;

        switch (state)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                break;
            case PlayerState.Moving:
                animator.Play("Walk");
                break;
            case PlayerState.Dead:
                animator.Play("Death");
                break;
            default:
                animator.Play("Idle");
                break;
        }

        currentState = state;
    }

    private void SetPlayerOrientation()
    {
        float horizontalInput = player.GetVelocity().x;
        if (horizontalInput > 0)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
    }
}
