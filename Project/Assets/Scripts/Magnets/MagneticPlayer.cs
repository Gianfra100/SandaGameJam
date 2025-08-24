using UnityEngine;

public class MagneticPlayer : MagneticEntity
{
    public Rigidbody2D Rb { get; private set; }

    private BasePlayer player;

    private GameObject lastMagneticSurface;
    protected void Awake()
    {
        Rb = GetComponentInParent<Rigidbody2D>();
        player = GetComponentInParent<BasePlayer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MagneticSurface"))
        {
            MagneticSurface magneticSurface = collision.GetComponent<MagneticSurface>();

            if (magneticSurface == null) return;

            if (magneticSurface.GetCharge() == GetCharge())
            {
                var direction = ((Vector2)transform.position - collision.ClosestPoint(transform.position)).normalized;
                Debug.Log("Repel from surface " + direction);
                player.RepelFromSurface(direction, magneticSurface.force);
            }
            else
            {
                if (lastMagneticSurface == collision.gameObject) return;

                var direction = (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
                player.AttractToSurface(direction.y);
                lastMagneticSurface = collision.gameObject;
            }
        }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     var magneticSurface = other.GetComponent<MagneticSurface>();

    //     if (magneticSurface == null) return;

    //     Vector2 baseDir = magneticSurface.isCeiling ? Vector2.up : Vector2.down;

    //     if (magneticSurface.charge == charge)
    //     {
    //         Rb.AddForce(-baseDir * magneticSurface.force, ForceMode2D.Impulse);
    //     }
    //     else
    //     {
    //         Rb.AddForce(baseDir * magneticSurface.force, ForceMode2D.Impulse);
    //     }
    // }
}
