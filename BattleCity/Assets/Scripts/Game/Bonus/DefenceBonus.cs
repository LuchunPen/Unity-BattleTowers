/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 07:55
*/

using System;
using UnityEngine;

public class DefenceBonus: BonusController 
{
    //public static readonly Uid64 UNIQ = "EE483EFF39465402";
    private BehDamage _damage;

	void Start () 
	{
        _damage = this.GetComponent<BehDamage>();
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && collider.tag == "Player") {
            IModifiable<BehDamage> bd = collider.GetComponent<IModifiable<BehDamage>>();

            if (bd != null) {
                bd.SetModificator(new Modificator<BehDamage>(0, _damage));
            }
            IsActivate = true;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.transform.SetParent(collider.transform);
            this.transform.localPosition = new Vector3(0, 0, 0);
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
