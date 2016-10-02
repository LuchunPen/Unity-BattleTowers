/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 21:16
*/

using System;
using UnityEngine;

public class WallController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "BE3FC0264BF87202";
    private MapObject _mapObj;

	void Start () 
	{
        _mapObj = this.GetComponent<MapObject>();
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) { dam.NoHealthEvent += NoHealthEventHandler; }
    }

    private void NoHealthEventHandler(object sender, EventArgs arg)
    {
        IDamageable dam = this.GetComponent<IDamageable>();
        if (dam != null) {
            dam.NoHealthEvent -= NoHealthEventHandler;
            BTGame.Current.Stage.UnregisterMapObject(_mapObj);
        }
        if (this.gameObject != null) {
            Destroy(this.gameObject);
        }
    }
}
