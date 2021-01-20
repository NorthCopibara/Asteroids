using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private PlayerModel        _playerData;
    [SerializeField] private PlayerView         _playerView;
    [SerializeField] private Transform          _playerSpawnPoint;

    [SerializeField] private MainMenuView       _mainMenuView;

    private GameState _gameState = GameState.Start;

    private List<IUpdatable> _updatables = new List<IUpdatable>();

    private void Start()
    {
        _mainMenuView.Init(StartGame);
    }

    public void StartGame()
    {
        InitPlayer();
        DisableMenu();
        SetStateGame(GameState.Game);
    }

    private void InitPlayer() 
    {
        PlayerController playerController = new PlayerController(_playerData, _playerView, _playerSpawnPoint);
        _updatables.Add(playerController);
    }

    private void DisableMenu() 
    {
        _mainMenuView.Disable();
        _mainMenuView.gameObject.SetActive(false);
    }

    private void SetStateGame(GameState state) 
    {
        _gameState = state;
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
