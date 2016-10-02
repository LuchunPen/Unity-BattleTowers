/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 10:24
*/

using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class BonusController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "FE3F27403FC7E202";
    [SerializeField]
    private float _timeToDestroy;
    public float TimeToDestroy
    {
        get { return _timeToDestroy; }
    }

    [SerializeField]
    private float _activeTime;
    public float ActiveTime
    {
        get { return _activeTime; }
    }

    private float _timeExpected;
    protected bool _isActivate;

	protected virtual void Update ()
	{
        _timeExpected += Time.deltaTime;
        if (!_isActivate) {
            if (_timeExpected >= _timeToDestroy) {
                Destroy(this.gameObject);
            }
        }
        else {
            if(_timeExpected >= _activeTime) {
                Destroy(this.gameObject);
            }
            
        }
	}
}
