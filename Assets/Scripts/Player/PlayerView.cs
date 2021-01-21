using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D    _rb;
    [SerializeField] private GameObject     _rocketFire;

    private bool    _isMove;
    private Action  _gameOver;

    public PlayerView Init(Transform spawnPosition) 
    {
        return Instantiate(this, spawnPosition.position, Quaternion.identity);
    }

    public void Repaint(Action gameOver) 
    {
        _gameOver = gameOver;
    }

    public void ResetView(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.identity;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
    }

    public void MovePlayer(float force) 
    {
        _rb.AddForce(transform.up * force * Time.deltaTime);
    }

    public void RotatePlayer(float speed) 
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }

    public void MovementAnimation(bool isMoving) 
    {
        if (_isMove != isMoving)
        {
            _isMove = isMoving;
            _rocketFire.SetActive(isMoving);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Asteroid")) 
        {
            if (_gameOver != null)
            {
                _gameOver.Invoke();
            }
            else 
            {
                Debug.LogError("Miss action game over");
            }
        }
    }
}
