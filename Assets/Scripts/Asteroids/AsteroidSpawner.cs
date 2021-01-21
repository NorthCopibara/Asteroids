using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner
{
    private AsteroidModel           _asteroidModel;
    private AsteroidView            _asteroidView;
    private DestructionAsteroidView _destructionAsteroidView;
    private SpawnerModel            _spawnerModel;
    private float                   _timeToSpawn;
    private Camera                  _camera;
    private List<Func<Vector2>>     _spawnZonesPosition = new List<Func<Vector2>>();

    public AsteroidSpawner(SpawnerModel spawnerModel, AsteroidModel asteroidModel, AsteroidView asteroidView,
                           DestructionAsteroidView destructionAsteroidView) 
    {
        _asteroidModel           = asteroidModel;
        _asteroidView            = asteroidView;
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
        int firstZone = UnityEngine.Random.Range(0, 4);
        int secondZone;

        do
        {
            secondZone = UnityEngine.Random.Range(0, 4);
        } while (firstZone == secondZone);

        var firstSpawnPosition = _spawnZonesPosition[firstZone].Invoke();
        var secondSpawnPosition = _spawnZonesPosition[secondZone].Invoke();

        SpawnAstetoid(firstSpawnPosition, secondSpawnPosition);
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

    private void SpawnAstetoid(Vector2 spawnPosition, Vector2 movePosition)
    {
        var asteroid = PoolManager.SpawnObject(_asteroidView.gameObject, spawnPosition, Quaternion.identity);
        var asteroidView = asteroid.GetComponent<AsteroidView>();
        asteroidView.Init(_asteroidModel, movePosition, _destructionAsteroidView);
    }
}
