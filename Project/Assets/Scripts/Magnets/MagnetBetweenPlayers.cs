using UnityEngine;

public class MagnetBetweenPlayers : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }

    protected void Awake()
    {
        Rb = GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerMagnet"))
        {
            var direction = collision.transform.position - transform.position.normalized;
            Rb.AddForce(direction * 10f, ForceMode2D.Impulse);
        }
    }
}
