using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private PlayerModel        _playerData;
    [SerializeField] private PlayerView         _playerView;
    [SerializeField] private Transform          _playerSpawnPoint;

    private GameState _gameState = GameState.Start;

    private List<IUpdatable> _updatables = new List<IUpdatable>();

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        InitPlayer();

        _gameState = GameState.Game;
    }

    private void InitPlayer() 
    {
        PlayerController playerController = new PlayerController(_playerData, _playerView, _playerSpawnPoint);
        _updatables.Add(playerController);
    }

    private void Update()
    {
        if (_gameState != GameState.Game) 
        {
            return;
        }

        _updatables.ForEach(x => x.Update());
    }
}
