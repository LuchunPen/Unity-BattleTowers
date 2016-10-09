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
    private Camera GameCamera;

    [SerializeField]
    private BonusController[] AwailableBonus;

    [SerializeField]
    private Stage[] GameStages;

    [SerializeField]
    private Stage _currentStage;
    public Stage Stage {
        get { return _currentStage; }
    }
    private int _currentStageIndex;

    private BulletPool _bPool;
    public BulletPool BulletPool {
        get {
            if (_bPool == null) { _bPool = new BulletPool(); }
            return _bPool;
        }
    }

    private bool _isPaused;

    void Awake()
    {
        if (_current == null) { 
            _current = this;
        }
        else { Destroy(this); }
    }

	void Start()
    {
        if (GameCamera == null) {
            GameCamera = Camera.main;
            GameCamera.orthographic = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(PlayerKeys.Pause)) {
            if (!_isPaused) {
                Time.timeScale = 0;
                _isPaused = true;

                Informer.Current.ShowInformer = true;
                Informer.Current.SetMMainInfo("Pause");
            }
            else {
                Time.timeScale = 1;
                _isPaused = false;

                Informer.Current.ShowInformer = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_currentStage == null) {
                CreateNextStage();
            }
        }
    }

    public BonusController GetRandomBonus()
    {
        int index = UnityEngine.Random.Range(0, AwailableBonus.Length);
        return AwailableBonus[index];
    }

    private void CreateNextStage()
    {
        if (_currentStage != null) {
            DestroyObject(_currentStage.gameObject);
        }
        _currentStage = Instantiate(GameStages[_currentStageIndex]);

        if (_currentStage != null) {
            int sizeX = _currentStage.Map.SizeX;
            int sizeY = _currentStage.Map.SizeY;
            GameCamera.transform.position = new Vector3(sizeX / 2, sizeY / 2, -10);
            GameCamera.orthographicSize = sizeX > sizeY ? sizeX / 2 : sizeY / 2;
        }
        SubscribeToStageEvents(Stage);
        Informer.Current.ShowInformer = false;
    }

    private void StageWinHandler(object sender, EventArgs arg)
    {
        Informer.Current.ShowInformer = true;
        Informer.Current.SetMMainInfo("Stage complete");

        UnSubscribeToStageEvents(Stage);
        _currentStageIndex++;
        if (_currentStageIndex >= GameStages.Length) { _currentStageIndex = 0; }
        Invoke("CreateNextStage", 3);
    }

    private void StageLoseHandler(object sender, EventArgs arg)
    {
        UnSubscribeToStageEvents(Stage);
        _currentStageIndex = 0;
        if (_currentStage != null) {
            DestroyObject(_currentStage.gameObject);
        }

        Informer.Current.ShowInformer = true;
        Informer.Current.SetMMainInfo("Press Space to start");
    }

    private void SubscribeToStageEvents(Stage stage)
    {
        if (stage != null) {
            UnSubscribeToStageEvents(stage);

            stage.WinEvent += StageWinHandler;
            stage.LoseEvent += StageLoseHandler;
        }
    }

    private void UnSubscribeToStageEvents(Stage stage)
    {
        if (stage != null) {
            stage.WinEvent -= StageWinHandler;
            stage.LoseEvent -= StageLoseHandler;
        }
    }
}
