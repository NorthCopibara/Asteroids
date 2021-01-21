using System.Collections;
using UnityEngine;

public class AsteroidView : MonoBehaviour, IPoolable
{
    private float                   _speed;
    private Vector3                 _movePosition;
    private DestructionAsteroidView _destructionAsteroidView;

    public void Init(AsteroidModel asteroidModel, Vector2 movePosition, DestructionAsteroidView destructionAsteroidView)
    {
        _speed = Random.Range(-asteroidModel.ForceRange, asteroidModel.ForceRange) + asteroidModel.MiddleForce;
        _movePosition = movePosition;
        _destructionAsteroidView = destructionAsteroidView;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        PoolManager.SpawnObject(_destructionAsteroidView.gameObject, transform.position);
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
