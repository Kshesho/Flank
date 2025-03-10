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

    Coroutine _powerupsRtn, _ammoCratesRtn, _healthPotionsRtn;
    Coroutine _bossPowerupsRtn;

#endregion

    public void StartSpawningPowerups()
    {
        StopSpawningPowerups();
        _spawning = true;
        
        _powerupsRtn = StartCoroutine(SpawnPowerupsRtn());
        _ammoCratesRtn = StartCoroutine(SpawnAmmoCratesRtn());
        _healthPotionsRtn = StartCoroutine(SpawnHealthPotionsRtn());
    }
    public void StopSpawningPowerups()
    {
        _spawning = false;
        if (_powerupsRtn != null) { StopCoroutine(_powerupsRtn); }
        if (_ammoCratesRtn != null) { StopCoroutine(_ammoCratesRtn); }
        if (_healthPotionsRtn != null) { StopCoroutine(_healthPotionsRtn); }
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
    public void HealthExplosion()
    {
        if (_healthPotionsRtn != null) { StopCoroutine(_healthPotionsRtn); }
        _healthPotionsRtn = StartCoroutine(SpawnAShitLoadOfHealthPotionsRtn());
    }
    IEnumerator SpawnAShitLoadOfHealthPotionsRtn()
    {
        float xPos;
        float yPos = _ySpawnPos;
        Vector2 spawnPos;

        //spawn potions at random positions in quick succession, a little further back each time
        for (int i = 50; i > 0; i--)
        {
            xPos = Random.Range(_xBounds * -1, _xBounds);
            yPos += 0.05f;
            spawnPos = new Vector2(xPos, yPos);    
            Instantiate(_healthPotionPref, spawnPos, Quaternion.identity);
            yield return HM.WaitTime(0.05f);
        }
    }

    public void SpawnBossPowerups()
    {
        StopSpawningBossPowerups();
        _bossPowerupsRtn = StartCoroutine(BossPowerupsRtn());
    }
    IEnumerator BossPowerupsRtn()
    {
        float wait = 2f;
        yield return HM.WaitTime(wait);

        //spawn 1 random powerup
        Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
        Instantiate(RandomPowerup(), spawnPos, Quaternion.identity);

        yield return HM.WaitTime(wait);
        //spawn 1 ammo crate
        spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
        Instantiate(_ammoCratePref, spawnPos, Quaternion.identity);

        yield return HM.WaitTime(wait);
        //10% chance to spawn health potion
        if (Random.Range(1, 101) <= 10)
        {
            spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
            Instantiate(_healthPotionPref, spawnPos, Quaternion.identity);
        }
    }

    public void StopSpawningBossPowerups()
    {
        if (_bossPowerupsRtn != null) { StopCoroutine(_bossPowerupsRtn); }
    }


}