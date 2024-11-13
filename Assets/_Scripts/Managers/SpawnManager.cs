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

    [SerializeField] PowerupSpawner _powerupSpawner;

    [SerializeField] float _spawnCooldown = 2;
    [SerializeField] GameObject _enemyPrefab;

    bool _spawning = true;
    float _xBounds = 9, _ySpawnPos = 7;

    [SerializeField] WaveSO[] _enemyWaves;
    int _wave = 0;
    [SerializeField] Transform _enemyContainer;
    int _waveTimer;
    Coroutine _waveTimerRtn;

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
        _powerupSpawner.StartSpawningPowerups();
        StartCoroutine(SpawnEnemyWavesRtn());
	}

    IEnumerator SpawnEnemyWavesRtn()
    {
        //break out of this if player dies

        foreach(var wave in _enemyWaves)
        {
            //start wave timer, increment wave #
            _wave++;
            _waveTimer = wave.waveTime;
            StartWaveTimer(_waveTimer);
            UIManager.Instance.NewWaveUI(_wave, wave.waveTime);

            //Spawn each enemy
            for (int i = 0; i < wave.enemies; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(_xBounds * -1, _xBounds), _ySpawnPos);
                Instantiate(_enemyPrefab, spawnPos, Quaternion.identity, _enemyContainer);
                yield return HM.WaitTime(wave.timeBetweenEnemies);
            }

            //Wait until the wave is finished...
            while (!WaveFinished())
            {
                yield return null;
            }

            //Turn off countdown if it's still running. Then pre-wave countdown
            if (_waveTimerRtn != null) StopCoroutine(_waveTimerRtn);
            yield return StartCoroutine(CountdownToNextWaveRtn());
        }
    }
    void StartWaveTimer(int waveTime)
    {
        if (_waveTimerRtn != null)
            StopCoroutine(WaveTimerRtn(waveTime));
        _waveTimerRtn = StartCoroutine(WaveTimerRtn(waveTime));
    }
    IEnumerator WaveTimerRtn(int waveTime)
    {
        for (int i = waveTime; i > 0; i--)
        {
            yield return HM.WaitTime(1);
            _waveTimer--;
            UIManager.Instance.UpdateWaveTimer(_waveTimer);
        }
        //set to null to track when this coroutine has stopped
        _waveTimerRtn = null;
    }
    IEnumerator CountdownToNextWaveRtn()
    {
        UIManager.Instance.NextWaveCountdown("3...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("2...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("1...");
        yield return HM.WaitTime(1);
    }

    void StopSpawning()
    {
        _spawning = false;
        _powerupSpawner.StopSpawningPowerups();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if the enemy container is empty</returns>
    bool WaveFinished()
    {
        if (_enemyContainer.transform.childCount < 1 ||
            _waveTimer <= 0)
            return true;

        return false;
    }


}