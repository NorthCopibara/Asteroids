using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [SerializeField] private PlayerModel                _playerData;
    [SerializeField] private PlayerView                 _playerView;
    [SerializeField] private Transform                  _playerSpawnPoint;

    [SerializeField] private Transform                  _poolsParent;

    [SerializeField] private BulletView                 _bulletView;
    [SerializeField] private BulletModel                _bulletModel;
    
    [SerializeField] private AsteroidView               _asteroidView;
    [SerializeField] private DestructionAsteroidView    _destructionAsteroidView;
    [SerializeField] private AsteroidModel              _asteroidModel;
    [SerializeField] private SpawnerModel               _spawnerModel;

    [SerializeField] private MainMenuView               _mainMenuView;

    private GameState _gameState = GameState.Start;

    private List<IUpdatable> _updatables = new List<IUpdatable>();

    private void Start()
    {
        _mainMenuView.Init(StartGame);
    }

    public void StartGame()
    {
        InitPlayer();
        InitAsteroidsPool();
        DisableMenu();
        SetStateGame(GameState.Game);
    }

    private void InitPlayer() 
    {
        var bulletPool = InitBulletPool();

        PlayerController playerController = new PlayerController(_playerData, _playerView, _playerSpawnPoint, bulletPool);

        _updatables.Add(playerController);
    }

    private BulletsPool InitBulletPool() 
    {
        return new BulletsPool(_bulletModel, _bulletView, _poolsParent);
    }

    private void InitAsteroidsPool() 
    {
        var asteroidPool = new AsteroidsPool(_asteroidModel, _asteroidView, _poolsParent, _spawnerModel, _destructionAsteroidView);
        _updatables.Add(asteroidPool);
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
