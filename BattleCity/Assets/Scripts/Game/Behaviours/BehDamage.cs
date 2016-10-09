/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 20:51
*/

using System;
using UnityEngine;

public class BehDamage: MonoBehaviour, IModifiable<BehDamage>
{
    //public static readonly Uid64 UNIQ = "EE3FBA4466FDA502";

    public event EventHandler NoHealthEvent;

    [SerializeField]
    private DamageType _incomingDamageType;
    public DamageType IncomingDamageType { get { return _incomingDamageType; } }

    [SerializeField]
    private int _health;
    public int Health {
        get { return _health; }
    }

    private Modificator<BehDamage> _mod;

    public virtual void SetDamage(Damage damage)
    {
        if (_mod != null && _mod.ModObject != null) {
            _mod.ModObject.SetDamage(damage);

            int h = _mod.ModObject.Health;
            if (h <= 0) { NoHealthTrigger(); }
            return;
        }

        if (damage.DType >= _incomingDamageType) {
            _health -= damage.Value;
        }
        if (_health <= 0) { NoHealthTrigger(); }
    }

    public void SetModificator(Modificator<BehDamage> modificator)
    {
        _mod = modificator;
    }

    protected void NoHealthTrigger()
    {
        EventHandler ev = NoHealthEvent;
        if (ev != null) { ev(this, null); }
    }
}
