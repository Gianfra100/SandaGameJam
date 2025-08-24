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

    public bool Change = false;

    protected virtual void Start()
    {
        SetSprite();
    }

    void Update()
    {
        if (Change)
        {
            ChangeCharge(GetCharge() == Charge.Positive ? Charge.Negative : Charge.Positive);
            Change = false;
        }
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

    public void ChangeCharge(Charge newCharge)
    {
        charge = newCharge;
        SetSprite();
        OnChargeChange?.Invoke();
    }
}


