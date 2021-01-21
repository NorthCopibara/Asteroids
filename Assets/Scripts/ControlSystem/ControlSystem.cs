using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlSystem : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerModel                _playerData;
    [SerializeField] private PlayerView                 _playerView;
    [SerializeField] private Transform                  _playerSpawnPoint;

    [Space]
    [SerializeField] private Transform                  _poolsParent;

    [Space]
    [Header("Bullet")]
    [SerializeField] private BulletView                 _bulletView;
    [SerializeField] private BulletModel                _bulletModel;

    [Space]
    [Header("Asteroids")]
    [SerializeField] private List<AsteroidView>         _asteroidsView;
    [SerializeField] private DestructionAsteroidView    _destructionAsteroidView;
    [SerializeField] private AsteroidModel              _asteroidModel;
    [SerializeField] private SpawnerModel               _spawnerModel;

    [Space]
    [Header("GUI")]
    [SerializeField] private MainMenuView               _mainMenuView;
    [SerializeField] private GameOverView               _gameOverView;

    private GameState _gameState = GameState.Start;
    private List<IUpdatable> _updatables = new List<IUpdatable>();
    private PlayerController _playerController;

    private void Start()
    {
        _mainMenuView.Init(StartGame);
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        InitPlayer();
        InitAsteroidsPool();
        DisableMenu();

        SetStateGame(GameState.Game);
    }

    public void EndGame() 
    {
        Time.timeScale = 0;

        _gameOverView.Init(RestartGame);
        _gameOverView.gameObject.SetActive(true);
        SetStateGame(GameState.End);
    }

    public void RestartGame() 
    {
        Time.timeScale = 1;

        _gameOverView.gameObject.SetActive(false);
        _playerController.Reset(_playerSpawnPoint);

        PoolManager.ReleaseAllObject();

        SetStateGame(GameState.Game);
    }

    private void InitPlayer() 
    {
        var bulletPool = InitBulletPool();

        _playerController = new PlayerController(_playerData, _playerView, _playerSpawnPoint, bulletPool, EndGame);

        _updatables.Add(_playerController);
    }

    private BulletsPool InitBulletPool() 
    {
        return new BulletsPool(_bulletModel, _bulletView, _poolsParent);
    }

    private void InitAsteroidsPool() 
    {
        var asteroidPool = new AsteroidsPool(_asteroidModel, _asteroidsView, _poolsParent, _spawnerModel, _destructionAsteroidView);
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
