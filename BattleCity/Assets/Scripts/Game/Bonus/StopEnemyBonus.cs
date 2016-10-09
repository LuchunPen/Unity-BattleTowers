/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 15:02
*/

using System;
using UnityEngine;

public class StopEnemyBonus : BonusController
{
    //public static readonly Uid64 UNIQ = "DE3F6882EF38D102";
    private BehFire _fire;
    private BehMove _move;

    void Start()
    {
        _fire = this.GetComponent<BehFire>();
        _move = this.GetComponent<BehMove>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && collider.tag == "Player") {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < gos.Length; i++) {
                IModifiable<BehFire> bf = gos[i].GetComponent<IModifiable<BehFire>>();
                IModifiable<BehMove> bm = gos[i].GetComponent<IModifiable<BehMove>>();
                if (bf != null) {
                    bf.SetModificator(new Modificator<BehFire>(0, _fire));
                }
                if (bm != null) {
                    bm.SetModificator(new Modificator<BehMove>(0, _move));
                }
            }
            IsActivate = true;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.transform.SetParent(collider.transform);
            this.transform.localPosition = new Vector3(0, 0, 0);
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
