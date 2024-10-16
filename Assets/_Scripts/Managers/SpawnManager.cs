#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class SpawnManager : MonoSingleton<SpawnManager> 
{
#region Variables

    [SerializeField] float _spawnCooldown = 2;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject[] _powerupPrefabs;
    [SerializeField] GameObject _ammoCratePref, _healthPotionPref;
    bool _spawning = true;
    float _xBounds = 9, _ySpawnPos = 7;

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


#endregion

    public void StartSpawning()
    {
	    StartCoroutine(SpawnEnemiesRtn());
        StartCoroutine(SpawnPowerupsRtn());
        StartCoroutine(SpawnAmmoCratesRtn());
        StartCoroutine(SpawnHealthPotionsRtn());
	}

    IEnumerator SpawnEnemiesRtn()
    {
        while (_spawning)
        {
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
            Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            yield return HM.WaitTime(_spawnCooldown);
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
    GameObject RandomPowerup()
    {
        int rand = Random.Range(0, _powerupPrefabs.Length);
        return _powerupPrefabs[rand];
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


}