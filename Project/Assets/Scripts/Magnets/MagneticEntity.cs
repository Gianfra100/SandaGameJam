using UnityEngine;

public enum Charge { Positive, Negative }

public abstract class MagneticEntity : MonoBehaviour
{
    public Charge charge = Charge.Positive;

    public virtual void ToggleCharge()
    {
        charge = (charge == Charge.Positive) ? Charge.Negative : Charge.Positive;
    }

    public Charge GetCharge()
    {
        return charge;
    }
}