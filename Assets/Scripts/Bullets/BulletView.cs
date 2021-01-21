using System;
using UnityEngine;

public class BulletView : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody2D    _rb;

    private Action<GameObject, GameObject>  _asteroidCollision;

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {

    }

    public void Shot(BulletModel bulletModel, Action<GameObject, GameObject> asteroinCollision) 
    {
        _rb.velocity = transform.up * bulletModel.ShotForce;
        _asteroidCollision = asteroinCollision;
    }

    private void OnBecameInvisible()
    {
        PoolManager.ReleaseObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Asteroid")) 
        {
            _asteroidCollision.Invoke(gameObject, collision.gameObject);
        }
    }
}
