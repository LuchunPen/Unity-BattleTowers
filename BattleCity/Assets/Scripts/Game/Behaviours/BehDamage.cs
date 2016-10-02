/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 20:51
*/

using System;
using UnityEngine;

public class BehDamage: MonoBehaviour, IDamageable
{
    //public static readonly Uid64 UNIQ = "EE3FBA4466FDA502";

    public event EventHandler NoHealthEvent;

    [SerializeField]
    private DamageType _incomingDamageType;
    public DamageType IncomingDamageType { get { return _incomingDamageType; } }

    [SerializeField]
    private int _health;
    public int Health { get { return _health; } }

    public void SetDamage(Damage damage)
    {
        if (damage.DType >= _incomingDamageType) {
            _health -= damage.Value;
        }

        if (_health <= 0) {
            if (NoHealthEvent != null) { NoHealthEvent(this, null); }
        }
    }
}
