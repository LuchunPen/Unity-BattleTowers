/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01/10/2016 21:42
*/

using System;
using UnityEngine;

public class BTGame : MonoBehaviour
{
    //public static readonly Uid64 UNIQ = "BE3E74D3DA1CF602";

    private static BTGame _current;
    public static BTGame Current
    {
        get { return _current; }
    }

    [SerializeField]
    private Stage _currentStage;
    public Stage Stage { get { return _currentStage; } }

    void Awake()
    {
        if (_current == null) { 
            _current = this;
        }
        else { Destroy(this); }
    }

	void Start()
    {

    }

    void Update()
    {

    }
}
