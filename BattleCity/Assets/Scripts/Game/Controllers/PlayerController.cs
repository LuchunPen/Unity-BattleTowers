/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 22:27
*/

using System;
using UnityEngine;

public class PlayerController: MonoBehaviour, IModifiable<BehMove>, IModifiable<BehFire>, ILevelUP
{
    //public static readonly Uid64 UNIQ = "AE3E7F55B71BD202";

    public event EventHandler PlayerDestroyEvent;

    [SerializeField]
    private MapObject[] _levelObjects;
    [SerializeField]
    private MapObject _activeLevelObject;

    [SerializeField]
    private int _level;
    public int Level {
        get { return _level; }
    }

    private BehMove _moveBehaviour;
    private BehFire _fireBehaviour;

    private Modificator<BehMove> _modMove;
    private Modificator<BehFire> _modFire;

    private Transform _bulletSpawner;

    void Awake()
    {
        if (_activeLevelObject == null) {
            _activeLevelObject = _levelObjects[0];
        }

        _moveBehaviour = _activeLevelObject.GetComponent<BehMove>();
        _fireBehaviour = _activeLevelObject.GetComponent<BehFire>();
        _bulletSpawner = _activeLevelObject.transform.FindChild("BulletSpawn");

    }

	void Update ()
	{
        Move();
        Fire();
        ChangeMoveDirection();
	}

    private void Move()
    {
        if (_modMove != null) {
            _modMove.ModTimer -= Time.deltaTime;
            if (_modMove.ModTimer > 0 && _modMove.ModObject != null) {
                _modMove.ModObject.Move(this.transform, _activeLevelObject);
            }
            else { _modFire = null; }
            return;
        }
        else if (_moveBehaviour != null) {
            _moveBehaviour.Move(this.transform, _activeLevelObject);
        }
    }

    private void Fire()
    {
        if (Input.GetKeyDown(PlayerKeys.Fire)) {
            if (_modFire != null) {
                _modFire.ModTimer -= Time.deltaTime;
                if (_modFire.ModTimer > 0 && _modFire.ModObject != null) {
                    _modFire.ModObject.Fire(_activeLevelObject.gameObject, _activeLevelObject.Direction, _bulletSpawner);
                }
                else { _modFire = null; }
                return;
            }
            if (_fireBehaviour != null) {
                _fireBehaviour.Fire(_activeLevelObject.gameObject, _activeLevelObject.Direction, _bulletSpawner);
            }
        }
    }

    private void ChangeMoveDirection()
    {
        if (_moveBehaviour == null) { return; }
        if (Input.GetKeyDown(PlayerKeys.MoveDown)) {
            _moveBehaviour.ChangeDirection(_activeLevelObject, Dir4.Down);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveUp)) {
            _moveBehaviour.ChangeDirection(_activeLevelObject, Dir4.Up);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveRight)) {
            _moveBehaviour.ChangeDirection(_activeLevelObject, Dir4.Right);
        }
        if (Input.GetKeyDown(PlayerKeys.MoveLeft)) {
            _moveBehaviour.ChangeDirection(_activeLevelObject, Dir4.Left);
        }
    }

    public void SetModificator(Modificator<BehMove> modificator)
    {
        if (modificator == null) { return; }
        _modMove = modificator;
    }

    public void SetModificator(Modificator<BehFire> modificator)
    {
        if (modificator == null) { return; }
        _modFire = modificator;
    }

    public void LevelUp()
    {
        int oldLevel = _level;
        _level++;
        if (_level >= _levelObjects.Length) {
            _level = _levelObjects.Length - 1;
        }
        if (oldLevel == _level) { return; }
        else {
            Dir4 dir = _activeLevelObject.Direction;
            _activeLevelObject.gameObject.SetActive(false);
            _activeLevelObject = _levelObjects[_level];
            _activeLevelObject.gameObject.SetActive(true);
            _moveBehaviour = _activeLevelObject.GetComponent<BehMove>();
            _fireBehaviour = _activeLevelObject.GetComponent<BehFire>();
            _bulletSpawner = _activeLevelObject.transform.FindChild("BulletSpawn");
            _activeLevelObject.Direction = dir;
        }
    }
}
