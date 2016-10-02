/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 06:10
*/
using System;

public interface ILevelUP
{
    void LevelUp();
}

public interface IModifiable<T>
{
    //public static readonly Uid64 UNIQ = "FE3EEBE7BC805902";
    void SetModificator(Modificator<T> modificator);
}

public class Modificator<T>
{
    private float _modTimer;
    public float ModTimer {
        get { return _modTimer; }
        set { _modTimer = value; }
    }

    private T _modObj;
    public T ModObject
    {
        get { return _modObj; }
    }

    public Modificator(float timer, T modObject)
    {
        _modObj = modObject;
        _modTimer = timer;
    }
}
