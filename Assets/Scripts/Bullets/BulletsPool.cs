using UnityEngine;

public class BulletsPool 
{
    private BulletModel _bulletModel;
    private BulletView  _bulletView;

    public BulletsPool(BulletModel bulletModel, BulletView bulletView, Transform poolsParent) 
    {
        _bulletModel    = bulletModel;
        _bulletView     = bulletView;

        PoolManager.SetRoot(poolsParent);
        PoolManager.WarmPool(_bulletView.gameObject, _bulletModel.BulletsCountInPool);
    }

    public void FireShot(Transform player) 
    {
        var bullet = PoolManager.SpawnObject(_bulletView.gameObject, player.position + Vector3.forward, player.transform.rotation);
        var bulletView = bullet.GetComponent<BulletView>();
        bulletView.Shot(_bulletModel, HittingAsteroid);
    }

    private void HittingAsteroid(GameObject bullet, GameObject asteroid) 
    {
        PoolManager.ReleaseObject(bullet);
        PoolManager.ReleaseObject(asteroid);
    }
}
