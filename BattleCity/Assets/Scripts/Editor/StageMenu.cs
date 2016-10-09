/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09/10/2016 08:39
*/

using System;
using UnityEngine;
using UnityEditor;

public static class StageMenu 
{
    //public static readonly Uid64 UNIQ = "EE484933F0838302";

    public static string TemplatePath = "Prefabs/StageTemplate";

    [MenuItem("Battle City/Create New Map")]
	public static void CreateNewStage()
    {
        GameObject.Instantiate(Resources.Load(TemplatePath, typeof(GameObject)));
    }
}
