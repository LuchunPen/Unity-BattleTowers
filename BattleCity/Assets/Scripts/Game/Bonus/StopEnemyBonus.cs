/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 15:02
*/

using System;
using UnityEngine;

public class StopEnemyBonus : BonusController
{
	//public static readonly Uid64 UNIQ = "DE3F6882EF38D102";
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
