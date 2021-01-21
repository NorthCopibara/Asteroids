using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPool : IUpdatable
{
    private AsteroidSpawner _spawner;

    public AsteroidsPool(AsteroidModel asteroidModel, List<AsteroidView> asteroidsView, Transform poolsParent, 
                         SpawnerModel spawnerModel, DestructionAsteroidView destructionAsteroidView)
    {
        PoolManager.SetRoot(poolsParent);
        foreach (var asterod in asteroidsView)
        {
            PoolManager.WarmPool(asterod.gameObject, asteroidModel.AsteroidsCountInPool);
        }
        PoolManager.WarmPool(destructionAsteroidView.gameObject, 5);

        _spawner = new AsteroidSpawner(spawnerModel, asteroidModel, GetAsteroidsByType(asteroidsView), destructionAsteroidView);
    }

    private static Dictionary<AsteroidsType, AsteroidView> GetAsteroidsByType(List<AsteroidView> asteroidsView)
    {
        Dictionary<AsteroidsType, AsteroidView> asteroids = new Dictionary<AsteroidsType, AsteroidView>();
        asteroids.Add(AsteroidsType.BigAsteroid, asteroidsView[0]);
        asteroids.Add(AsteroidsType.MiniAsteroid, asteroidsView[1]);
        asteroids.Add(AsteroidsType.MicroAsteroid, asteroidsView[2]);
        return asteroids;
    }

    public void Update()
    {
        if (_spawner.UpdateSpawn()) 
        {
            _spawner.InitAsteroid();
        }
    }
}
