/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 14:36
*/

using System;
using UnityEngine;

public class LevelUpBonus : BonusController
{
    //public static readonly Uid64 UNIQ = "CE3F626854C75002";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null) {
            ILevelUP lvl = collider.GetComponent<ILevelUP>();
            if (lvl != null) {
                lvl.LevelUp();
                _isActivate = true;
            }
        }
    }
}
