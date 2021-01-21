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

    public PlayerController(PlayerModel playerModel, PlayerView playerView, Transform spawnPoint, BulletsPool bulletsPool, Action gameOver) 
    {
        _playerModel    = playerModel;
        _playerView     = playerView.Init(spawnPoint);

        _bulletsPool    = bulletsPool;

        _playerView.Repaint(gameOver);
    }

    public void Reset(Transform spawnPoint) 
    {
        _playerView.transform.position = spawnPoint.position;
        _playerView.transform.rotation = Quaternion.identity;
    }

    private void MovePlayer() 
    {
        bool isMoving = false;

        switch (_gameInput.GetInputForMove()) 
        {
            case MoveDirection.Forward:
                _playerView.MovePlayer(_playerModel.EngineForce);
                isMoving = true;
                break;
            case MoveDirection.Left:
                _playerView.RotatePlayer(_playerModel.SpeedRotation, 1);
                break;
            case MoveDirection.Right:
                _playerView.RotatePlayer(_playerModel.SpeedRotation, -1);
                break;
        }

        _playerView.MovementAnimation(isMoving);
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
