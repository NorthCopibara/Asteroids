using System;
using UnityEngine;

public class PlayerController : IUpdatable
{
    private GameInput _gameInput = new GameInput();

    private PlayerModel _playerModel;
    private PlayerView  _playerView;

    private BulletsPool _bulletsPool;

    private bool _shotStoper = false;
    private float _timeToNextShot = 0;

    public PlayerController(PlayerModel playerModel, PlayerView playerView, Transform spawnPoint, BulletsPool bulletsPool) 
    {
        _playerModel    = playerModel;
        _playerView     = playerView.Init(spawnPoint);

        _bulletsPool    = bulletsPool;
    }

    private void MovePlayer() 
    {
        switch (_gameInput.GetInputForMove()) 
        {
            case MoveDirection.Forward:
                _playerView.Rb.AddForce(_playerView.transform.up * _playerModel.EngineForce * Time.deltaTime);
                break;
            case MoveDirection.Left:
                _playerView.transform.Rotate(Vector3.forward, _playerModel.SpeedRotation * Time.deltaTime);
                break;
            case MoveDirection.Right:
                _playerView.transform.Rotate(-Vector3.forward, _playerModel.SpeedRotation * Time.deltaTime);
                break;
        }
    }

    private void PlayerAttack() 
    {
        if (_gameInput.GetInputForAttack() && !_shotStoper) 
        {
            _bulletsPool.FireShot(_playerView.gameObject.transform);
            _shotStoper = true;
        }
    }

    private void WaitingNewShot() 
    {
        if (_shotStoper) 
        {
            _timeToNextShot += Time.deltaTime;

            if (_timeToNextShot > _playerModel.TimeBetweenShots) 
            {
                _timeToNextShot = 0;
                _shotStoper = false;
            }
        }
    }

    public void Update()
    {
        MovePlayer();
        PlayerAttack();
        WaitingNewShot();
    }
}
