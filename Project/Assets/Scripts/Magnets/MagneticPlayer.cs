using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MagneticPlayer : MagneticEntity
{
    public Rigidbody2D Rb { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Rb = GetComponent<Rigidbody2D>();
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
