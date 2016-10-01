/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:27
*/

using System;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "AE3E7F55B71BD202";

    private MapObject _mapObj;
    private BehMove _moveBehaviour;
    private BehFire _fireBehaviour;

    void Awake()
    {
        _mapObj = this.GetComponent<MapObject>();
        _moveBehaviour = this.GetComponent<BehMove>();
        _fireBehaviour = this.GetComponent<BehFire>();
    }
	
	void Update ()
	{
        if (_moveBehaviour != null) {
            _moveBehaviour.Move();
        }

        Fire();
        ChangeMoveDirection();
	}

    private void Fire()
    {
        if (_fireBehaviour == null) { return; }
        if (Input.GetKeyDown(PlayerKeys.Fire)) {
            _fireBehaviour.Fire(_mapObj.Direction);
        }
    }

    private void ChangeMoveDirection()
    {
        if (_moveBehaviour == null) { return; }
        if (Input.GetKeyDown(PlayerKeys.MoveDown)) {
            _moveBehaviour.ChangeDirection(Dir4.Down);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveUp)) {
            _moveBehaviour.ChangeDirection(Dir4.Up);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveRight)) {
            _moveBehaviour.ChangeDirection(Dir4.Right);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveLeft)) {
            _moveBehaviour.ChangeDirection(Dir4.Left);
        }
    }
}
