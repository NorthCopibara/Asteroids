using System;
using UnityEngine;

public class AsteroidView : MonoBehaviour, IPoolable
{
    private float                            _speed;
    private Vector3                          _movePosition;
    private DestructionAsteroidView          _destructionAsteroidView;
    private AsteroidsType                    _asteroidType;
    private Action<Transform, AsteroidsType> _spawnSmallAsteroid;

    public void Init(AsteroidModel asteroidModel, Vector2 movePosition, DestructionAsteroidView destructionAsteroidView, 
                     AsteroidsType asteroidsType, Action<Transform, AsteroidsType> spawnSmallAsteroid)
    {
        _speed = UnityEngine.Random.Range(-asteroidModel.ForceRange, asteroidModel.ForceRange) + asteroidModel.MiddleForce;
        _movePosition = movePosition;
        _destructionAsteroidView = destructionAsteroidView;
        _asteroidType = asteroidsType;
        _spawnSmallAsteroid = spawnSmallAsteroid;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        _asteroidType = AsteroidsType.BigAsteroid;

        SpawnSmallAsteroids();
        PoolManager.SpawnObject(_destructionAsteroidView.gameObject, transform.position);
    }

    private void SpawnSmallAsteroids() 
    {
        if (_asteroidType != AsteroidsType.MicroAsteroid) 
        {
            if (_spawnSmallAsteroid != null) 
            {
                _spawnSmallAsteroid.Invoke(gameObject.transform, _asteroidType + 1);
            }
        }
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _movePosition) < 0.001f) 
        {
            PoolManager.ReleaseObject(gameObject);
        }
    }
}
