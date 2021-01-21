using UnityEngine;

public class AsteroidsPool : IUpdatable
{
    private AsteroidSpawner _spawner;

    public AsteroidsPool(AsteroidModel asteroidModel, AsteroidView asteroidView, Transform poolsParent, SpawnerModel spawnerModel,
                         DestructionAsteroidView destructionAsteroidView) 
    {
        PoolManager.SetRoot(poolsParent);
        PoolManager.WarmPool(asteroidView.gameObject, asteroidModel.AsteroidsCountInPool);
        PoolManager.WarmPool(destructionAsteroidView.gameObject, 5);

        _spawner = new AsteroidSpawner(spawnerModel, asteroidModel, asteroidView, destructionAsteroidView);
    }

    public void Update()
    {
        if (_spawner.UpdateSpawn()) 
        {
            _spawner.InitAsteroid();
        }
    }
}
