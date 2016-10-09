/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 10:00
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class Informer: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "CE485C228B895902";

    private static Informer _current;
    public static Informer Current {
        get { return _current; }
    }

    [SerializeField]
    private Text text;

    private bool _show = true;
    public bool ShowInformer {
        get {
            return _show;
        }
        set {
            if (value == _show) { return; }
            if (value) {
                text.enabled = true;
            }
            else {
                text.enabled = false;
            }
            _show = value;
        }
    }

    void Awake()
    {
        if (_current == null) { _current = this; }
        else { Destroy(this); }

        if (text == null) {
            text = this.GetComponent<Text>();
        }
    }

    public void SetMMainInfo(string info)
    {
        text.text = info;
    }
}
