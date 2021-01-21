using System;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidsType 
{
    BigAsteroid,
    MiniAsteroid,
    MicroAsteroid
}

public class AsteroidSpawner
{
    private AsteroidModel                           _asteroidModel;
    private Dictionary<AsteroidsType, AsteroidView> _asteroidsView = new Dictionary<AsteroidsType, AsteroidView>();
    private DestructionAsteroidView                 _destructionAsteroidView;
    private SpawnerModel                            _spawnerModel;
    private float                                   _timeToSpawn;
    private Camera                                  _camera;
    private List<Func<Vector2>>                     _spawnZonesPosition = new List<Func<Vector2>>();

    public AsteroidSpawner(SpawnerModel spawnerModel, AsteroidModel asteroidModel, Dictionary<AsteroidsType, AsteroidView> asteroidsView,
                           DestructionAsteroidView destructionAsteroidView) 
    {
        _asteroidModel           = asteroidModel;
        _asteroidsView           = asteroidsView;
        _destructionAsteroidView = destructionAsteroidView;
        _spawnerModel            = spawnerModel;
        _camera                  = Camera.main;

        InitSpawnZonesPosition();
    }

    public bool UpdateSpawn()
    {
        _timeToSpawn += Time.deltaTime;

        if (_timeToSpawn > _spawnerModel.SpawnTimestamp) 
        {
            _timeToSpawn = 0;

            return true;
        }

        return false;
    }

    public void InitAsteroid()
    {
        int firstZoneNumber = UnityEngine.Random.Range(0, 4);
        int secondZoneNumber;

        do
        {
            secondZoneNumber = UnityEngine.Random.Range(0, 4);
        } while (firstZoneNumber == secondZoneNumber);

        var firstSpawnPosition = _spawnZonesPosition[firstZoneNumber].Invoke();
        var secondSpawnPosition = _spawnZonesPosition[secondZoneNumber].Invoke();

        SpawnAstetoid(firstSpawnPosition, secondSpawnPosition, AsteroidsType.BigAsteroid);
    }

    public void InitSmallAsteroids(Transform startPosition, AsteroidsType typeAsteroid) 
    {
        int asteroidsCount = UnityEngine.Random.Range(0, _spawnerModel.MaxSmallAsteroidsCount + 1);

        for (int i = 0; i < asteroidsCount; i++)
        {
            int zoneNumber = UnityEngine.Random.Range(0, 4);
            var spawnPosition = _spawnZonesPosition[zoneNumber].Invoke();

            SpawnAstetoid(startPosition.position, spawnPosition, typeAsteroid);
        }
    }

    private void InitSpawnZonesPosition()
    {
        _spawnZonesPosition.Add(() => DownZoneSpawn());
        _spawnZonesPosition.Add(() => UpZoneSpawn());
        _spawnZonesPosition.Add(() => LeftZoneSpawn());
        _spawnZonesPosition.Add(() => RigeZoneSpawn());
    }

    private Vector2 DownZoneSpawn()
    {
        float spawnY = _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).y - 1;
        float spawnX = UnityEngine.Random.Range(_camera.ScreenToWorldPoint(new Vector2(1, 1)).x,
                                                _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(spawnX, spawnY);
    }

    private Vector2 UpZoneSpawn()
    {
        float spawnY = _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1;
        float spawnX = UnityEngine.Random.Range(_camera.ScreenToWorldPoint(new Vector2(1, 1)).x,
                                                _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(spawnX, spawnY);
    }

    private Vector2 LeftZoneSpawn()
    {
        float spawnY = UnityEngine.Random.Range(_camera.ScreenToWorldPoint(new Vector2(0, 0)).y,
                                                _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).x - 1;

        return new Vector2(spawnX, spawnY);
    }

    private Vector2 RigeZoneSpawn()
    {
        float spawnY = UnityEngine.Random.Range(_camera.ScreenToWorldPoint(new Vector2(0, 0)).y,
                                                _camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = _camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;

        return new Vector2(spawnX, spawnY);
    }

    private void SpawnAstetoid(Vector2 spawnPosition, Vector2 movePosition, AsteroidsType asteroidsType)
    {
        var asteroid = PoolManager.SpawnObject(_asteroidsView[asteroidsType].gameObject, spawnPosition, Quaternion.identity);
        var asteroidView = asteroid.GetComponent<AsteroidView>();
        asteroidView.Init(_asteroidModel, movePosition, _destructionAsteroidView, asteroidsType, InitSmallAsteroids);
    }
}
