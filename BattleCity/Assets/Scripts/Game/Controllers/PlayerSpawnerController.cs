/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02/10/2016 11:22
*/

using System;
using UnityEngine;

public class PlayerSpawnerController: MonoBehaviour 
{
    //public static readonly Uid64 UNIQ = "CE3F3513A9128F02";
    [SerializeField]
    public PlayerController _playerPref;
    private PlayerController _activePlayer;

	void Start () 
	{
        this.GetComponent<SpriteRenderer>().enabled = false;
	    if (_activePlayer == null) {
            _activePlayer = Instantiate(_playerPref, this.transform.position, this.transform.rotation) as PlayerController;
            _activePlayer.PlayerDestroyEvent += OnPlayerDestroyEventHandler;

        }
	}

    private void OnPlayerDestroyEventHandler(object sender, EventArgs arg)
    {
        _activePlayer.PlayerDestroyEvent -= OnPlayerDestroyEventHandler;
    }
}
