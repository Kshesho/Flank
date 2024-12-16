#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class PowerupSpawner : MonoBehaviour 
{
#region Variables

    bool _spawning = true;
    float _xBounds = 9, _ySpawnPos = 7;
    [Header("Ammo Crate")]
    [Tooltip("Pwerups are spawned at a random interval between these minimum (X) and maximum (Y) values, in seconds.")]
    [SerializeField] Vector2 _ammo_MinMaxSpawnFrequency = new Vector2(8f, 14f);
    [SerializeField] GameObject _ammoCratePref;

    [Header("Health Potion")]
    [Tooltip("Pwerups are spawned at a random interval between these minimum (X) and maximum (Y) values, in seconds.")]
    [SerializeField] Vector2 _health_MinMaxSpawnFrequency = new Vector2(35f, 40f);
    [SerializeField] GameObject _healthPotionPref;

    [Header("All Other Powerups")]
    [Tooltip("Pwerups are spawned at a random interval between these minimum (X) and maximum (Y) values, in seconds.")]
    [SerializeField] Vector2 _other_MinMaxSpawnFrequency = new Vector2(4f, 10f);
    [SerializeField] GameObject[] _powerupPrefabs;
    [SerializeField] int[] _powerupSpawnWeights;

#endregion

    public void StartSpawningPowerups()
    {
        StartCoroutine(SpawnPowerupsRtn());
        StartCoroutine(SpawnAmmoCratesRtn());
        StartCoroutine(SpawnHealthPotionsRtn());
    }
    public void StopSpawningPowerups()
    {
        _spawning = false;
    }

//==========================================================================

    IEnumerator SpawnPowerupsRtn()
    {
        while (_spawning)
        {
            float rand = Random.Range(_other_MinMaxSpawnFrequency.x, _other_MinMaxSpawnFrequency.y);
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
            float rand = Random.Range(_ammo_MinMaxSpawnFrequency.x, _ammo_MinMaxSpawnFrequency.y);
            yield return HM.WaitTime(rand);
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);

            Instantiate(_ammoCratePref, spawnPos, Quaternion.identity);
        }
    }

    IEnumerator SpawnHealthPotionsRtn()
    {
        while (_spawning)
        {
            float rand = Random.Range(_health_MinMaxSpawnFrequency.x, _health_MinMaxSpawnFrequency.y);
            yield return HM.WaitTime(rand);
            Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);

            Instantiate(_healthPotionPref, spawnPos, Quaternion.identity);
        }
    }


}