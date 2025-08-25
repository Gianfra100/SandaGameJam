using System;
using Unity.VisualScripting;
using UnityEngine;

public class MagneticSurface : MagneticEntity
{
    public float force = 30f;
    public bool isCeiling = false;

    public GameObject PositePlatform;
    public GameObject NegativePlatform;

    public event Action OnChargeChange;

    protected virtual void Start()
    {
        SetSprite();
    }

    private void SetSprite()
    {
        if (PositePlatform == null || NegativePlatform == null) return;

        if (GetCharge() == Charge.Positive)
        {
            PositePlatform.SetActive(true);
            NegativePlatform.SetActive(false);
        }
        else
        {
            PositePlatform.SetActive(false);
            NegativePlatform.SetActive(true);
        }
    }

    public virtual void ToggleCharge()
    {
        charge = (charge == Charge.Positive) ? Charge.Negative : Charge.Positive;
        SetSprite();    
        OnChargeChange?.Invoke();
    }
}


