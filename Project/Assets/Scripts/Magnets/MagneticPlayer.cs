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
}
