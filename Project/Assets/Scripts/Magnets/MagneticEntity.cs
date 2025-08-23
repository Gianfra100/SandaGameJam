using UnityEngine;

public enum Charge { Positive, Negative }

public abstract class MagneticEntity : MonoBehaviour
{
    public Charge charge = Charge.Positive;
    [SerializeField] protected SpriteRenderer spriteRendered;

    protected virtual void Awake()
    {
        if (spriteRendered == null)
        {
            spriteRendered = GetComponent<SpriteRenderer>();
            if (spriteRendered == null) spriteRendered = GetComponentInChildren<SpriteRenderer>();
            if (spriteRendered == null) spriteRendered = GetComponentInParent<SpriteRenderer>();
        }

        UpdateColor();
    }

    public virtual void ToggleCharge()
    {
        charge = (charge == Charge.Positive) ? Charge.Negative : Charge.Positive;
        UpdateColor();
    }

    protected virtual void UpdateColor()
    {
        if (spriteRendered == null) return;

        spriteRendered.color = (charge == Charge.Positive) ? Color.blue : Color.red;
    }
}