using UnityEngine;

public enum Charge { Positive, Negative }

public abstract class MagneticEntity : MonoBehaviour
{
    public Charge charge = Charge.Positive;
    [SerializeField] protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
            if (sr == null) sr = GetComponentInParent<SpriteRenderer>();
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
        if (sr == null) return;

        sr.color = (charge == Charge.Positive) ? Color.blue : Color.red;
    }
}