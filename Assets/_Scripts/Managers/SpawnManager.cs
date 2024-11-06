using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (responsibility of this class)
/// </summary>
public class SpawnManager : MonoSingleton<SpawnManager> 
{
#region Variables

    [SerializeField] float _spawnCooldown = 2;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject[] _powerupPrefabs;
    [SerializeField] int[] _powerupSpawnWeights;
    [SerializeField] GameObject _ammoCratePref, _healthPotionPref;
    bool _spawning = true;
    float _xBounds = 9, _ySpawnPos = 7;

    [SerializeField] WaveSO[] _enemyWaves;
    int _wave = 0;
    [SerializeField] Transform _enemyContainer;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnPlayerDeath += StopSpawning;
    }

    void OnDisable()
    {
        Events.OnPlayerDeath -= StopSpawning;
    }

    void Update()
    {

    }

#endregion

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerupsRtn());
        StartCoroutine(SpawnAmmoCratesRtn());
        StartCoroutine(SpawnHealthPotionsRtn());
	    
        //Start spawning the first wave of enemies
        //spawn them into a container 
        //when that container is empty, start the next wave
        StartCoroutine(SpawnEnemyWavesRtn());
	}

    IEnumerator SpawnEnemyWavesRtn()
    {
        //break out of this if player dies

        foreach(var wave in _enemyWaves)
        {
            //start wave timer, increment wave #
            _wave++;
            print("Wave " + _wave);

            //for each enemy to spawn...
            for (int i = 0; i < wave.enemies; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
                Instantiate(_enemyPrefab, spawnPos, Quaternion.identity, _enemyContainer);
                yield return HM.WaitTime(wave.timeBetweenEnemies);
            }

            //After wave has finished spawning, wait until all enemies are dead
            while (!WaveFinished())
            {
                print("waiting for wave to finish...");
                yield return null;
            }
        }
    }

    IEnumerator SpawnPowerupsRtn()
    {
        while (_spawning)
        {
            float rand = Random.Range(4f, 10f);
            yield return HM.WaitTime(rand);
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
            
            Instantiate(RandomPowerup(), spawnPos, Quaternion.identity);
        }
    }
    /// <summary>
    /// </summary>
    /// <returns>A random powerup, selected by drop chance.</returns>
    GameObject RandomPowerup()
    {
        int total = 0;
        foreach(var weight in _powerupSpawnWeights)
        {
            total += weight;
        }
        int randomWeight = Random.Range(1, total);

        for (int i = 0; i < _powerupPrefabs.Length; i++)
        {
            if (randomWeight <= _powerupSpawnWeights[i])
            {
                return _powerupPrefabs[i];
            }
            else randomWeight -= _powerupSpawnWeights[i];
        }

        return null;
    }

    IEnumerator SpawnAmmoCratesRtn()
    {
        while (_spawning)
        {
            float rand = Random.Range(8f, 14f);
            yield return HM.WaitTime(rand);
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);

            Instantiate(_ammoCratePref, spawnPos, Quaternion.identity);
        }
    }

    IEnumerator SpawnHealthPotionsRtn()
    {
        while (_spawning)
        {
            yield return HM.WaitTime(40);
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);

            Instantiate(_healthPotionPref, spawnPos, Quaternion.identity);
        }
    }

    void StopSpawning()
    {
        _spawning = false;
        //could also stop coroutine here
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if the enemy container is empty</returns>
    bool WaveFinished()
    {
        if (_enemyContainer.transform.childCount < 1)
            return true;

        return false;
    }


}