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

    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] GameObject _bossFightGO;

    Vector2 _defaultSpawnPos = new Vector2(0, 7);

    [SerializeField] WaveSO[] _enemyWaves;
    int _wave = 0;
    public int CurrentWave { get { return _wave; } }
    [SerializeField] Transform _enemyContainer;
    int _waveTimer;
    Coroutine _waveTimerRtn, _spawnRateTimerRtn;

    //Endgame
    float _curSpawnInterval = 4f, _baseSpawnInterval;

#endregion
#region Base Methods

    void Startt()
    {
        _baseSpawnInterval = _curSpawnInterval;
    }
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
        for (int i = 0; i < _enemyWaves.Length; i++)
        {
            var wave = _enemyWaves[i];

            //start wave timer, increment wave #
            _wave++;
            _waveTimer = wave.waveTime;
            //don't start wave timer if last wave (before boss)
            if (i == _enemyWaves.Length - 1)
            {
                UIManager.Instance.LastWaveUI();
            }
            else
            {
                StartWaveTimer(_waveTimer);
                UIManager.Instance.NewWaveUI(_wave, wave.waveTime);
            }

            //Spawn each enemy
            for (int j = 0; j < wave.enemies.Length; j++)
            {
                //convert the waves EnemyType from enum to int
                int index = (int)wave.enemies[j];
                Instantiate(_enemyPrefabs[index], _defaultSpawnPos, Quaternion.identity, _enemyContainer);
                yield return HM.WaitTime(wave.timeBetweenEnemies);
            }

            //Wait until the wave is finished...
            while (!WaveFinished()) 
                yield return null;

            ///If I'm on the last wave, 
            /// don't start wave timer
            /// once post-countdown is complete,
            /// spawn boss

            //Turn off countdown if it's still running. Then pre-wave countdown
            if (_waveTimerRtn != null) StopCoroutine(_waveTimerRtn);
            yield return StartCoroutine(CountdownToNextWaveRtn());
        }

        SpawnBoss();
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
        UIManager.Instance.NextWaveCountdown("5...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("4...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("3...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("2...");
        yield return HM.WaitTime(1);
        UIManager.Instance.NextWaveCountdown("1...");
        yield return HM.WaitTime(1);
    }

    #region Endgame Spawning

    public void StartEndgameSpawning()
    {
        _powerupSpawner.StartSpawningPowerups();
        StartCoroutine(EndgameSpawningRtn());
        StartCoroutine(DecreaseSpawnIntervalRtn());
    }
    IEnumerator EndgameSpawningRtn()
    {
        yield return HM.WaitTime(2);
        int index;
        float baseDelay = _curSpawnInterval;

        while (!GameManager.Instance.GameOver)
        {
            //pick a random enemy
            index = Random.Range(0, _enemyPrefabs.Length);
            Instantiate(_enemyPrefabs[index], _defaultSpawnPos, Quaternion.identity, _enemyContainer);

            yield return HM.WaitTime(_curSpawnInterval);
        }
    }
    IEnumerator DecreaseSpawnIntervalRtn()
    {
        int waitTime = 30;
        while (!GameManager.Instance.GameOver)
        {
            if (_spawnRateTimerRtn != null) { StopCoroutine(_spawnRateTimerRtn); }
            _spawnRateTimerRtn = StartCoroutine(SpawnRateTimerRtn(waitTime));

            yield return HM.WaitTime(waitTime);
            _curSpawnInterval *= 0.95f;
            float newSpawnRate = 4 / _curSpawnInterval;
            UIManager.Instance.UpdateSpawnRate(newSpawnRate);
        }
    }
    IEnumerator SpawnRateTimerRtn(int time)
    {
        for (int i = time; i > 0; i--)
        {
            UIManager.Instance.UpdateWaveTimer(i);
            yield return HM.WaitTime(1);
        }
    }

    #endregion

    #region Boss

    void SpawnBoss()
    {
        UIManager.Instance.BossUI();
        _powerupSpawner.StopSpawningPowerups();
        HealthPotionExplosion();
        AudioManager.Instance.StopMusic_BattleTheme();
        StartCoroutine(SpawnBossAfterWaitRtn());
    }
    IEnumerator SpawnBossAfterWaitRtn()
    {
        //give time for health potions to spawn and move off-screen
        yield return HM.WaitTime(21);
        _bossFightGO.SetActive(true);
        AudioManager.Instance.PlayMusic_BossTheme();
    }

    public void HealthPotionExplosion()
    {
        _powerupSpawner.HealthExplosion();
    }

    /// <summary>
    /// Resumes powerup spawning that happens during the boss fight.
    /// </summary>
    public void StartSpawningPowerups_Boss()
    {
        _powerupSpawner.SpawnBossPowerups();
    }
    /// <summary>
    /// Stops the powerup spawning that happens during the boss fight.
    /// </summary>
    public void StopSpawningPowerups_Boss()
    {
        _powerupSpawner.StopSpawningBossPowerups();
    }

    #endregion

    void StopSpawning()
    {
        StopAllCoroutines();
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