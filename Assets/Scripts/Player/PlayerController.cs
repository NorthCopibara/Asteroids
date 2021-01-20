using System;
using UnityEngine;

public class PlayerController : IUpdatable
{
    private GameInput _gameInput = new GameInput();

    private PlayerModel _playerModel;
    private PlayerView  _playerView;

    public PlayerController(PlayerModel playerModel, PlayerView playerView, Transform spawnPoint) 
    {
        _playerModel    = playerModel;
        _playerView     = playerView.Init(spawnPoint);
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
        if (_gameInput.GetInputForAttack()) 
        {
        
        }
    }

    public void Update()
    {
        MovePlayer();
        PlayerAttack();
    }
}
