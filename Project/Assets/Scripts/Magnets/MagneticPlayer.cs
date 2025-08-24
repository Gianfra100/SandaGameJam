using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BasePlayer))]
public class MagneticPlayer : MagneticEntity
{
    public Rigidbody2D Rb { get; private set; }

    private BasePlayer player;
    protected void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        player = GetComponent<BasePlayer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var magneticSurface = other.GetComponent<MagneticSurface>();
        if (magneticSurface == null) return;

        Vector2 baseDir = magneticSurface.isCeiling ? Vector2.up : Vector2.down;

        if (magneticSurface.charge == charge)
        {
            Rb.AddForce(-baseDir * magneticSurface.force, ForceMode2D.Impulse);
        }
        else
        {
            Rb.AddForce(baseDir * magneticSurface.force, ForceMode2D.Impulse);
        }
    }
}
