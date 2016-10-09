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

    [SerializeField]
    private float _timeExpected;
    private bool _isActivate;
    public bool IsActivate {
        get { return _isActivate; }
        protected set {
            _isActivate = value;
            _timeExpected = 0;
        }
    }

	protected virtual void Update ()
	{
        _timeExpected += Time.deltaTime;
        if (!_isActivate) {
            if (_timeExpected == 0) { return; }
            float t = _timeExpected / _timeToDestroy;
            if (t > 0.6f) {
                float s = 1 - t + 0.6f;
                this.transform.localScale = new Vector3(s, s, s);
            }
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
