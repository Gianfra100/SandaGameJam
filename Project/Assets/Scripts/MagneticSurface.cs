using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MagneticSurface : MagneticEntity
{
    [SerializeField] private float force = 10f;

    private void Reset()
    {
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<MagneticPlayer>();
        if (player == null) return;

        Vector2 dir = (player.transform.position - transform.position).normalized;

        if (player.charge == charge)
        {
            player.Rb.AddForce(dir * force, ForceMode2D.Force);
        }
        else
        {
            player.Rb.AddForce(-dir * force, ForceMode2D.Force);
        }
    }
}
