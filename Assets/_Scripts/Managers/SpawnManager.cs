#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class SpawnManager : MonoBehaviour 
{
#region Variables

    [SerializeField] float _spawnCooldown = 2;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _javelinPowerupPrefab;
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

    void Start () 
    {
	    StartCoroutine(SpawnEnemiesRtn());
        StartCoroutine(SpawnPowerupsRtn());
	}

#endregion

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
            //TODO: choose random powerup when there are more
            Instantiate(_javelinPowerupPrefab, spawnPos, Quaternion.identity);
        }
    }

    void StopSpawning()
    {
        _spawning = false;
        //could also stop coroutine here
    }


}