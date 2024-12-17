#region Using Statements
using System.Collections;
using UnityEngine;
using Narzioth.Utilities;
#endregion

/// <summary>
/// Holds Enemy health and damage functionality.
/// </summary>
public class EnemyHeart : MonoBehaviour 
{
#region Variables

    [SerializeField] EnemyMovement _enemyMovement;
    [SerializeField] EnemyAnimStateChanger _animStateChanger;

    [SerializeField] int _health = 10;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] Collider2D _thisCollider;
    bool _shieldActive;
    [SerializeField] GameObject _shieldGO;

#endregion
#region Base Methods

    void Start()
    {
        RollForShield();
    }
    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
    }

    private void OnDisable()
    {
        Events.OnCollide -= TakeDamage;
    }

#endregion

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider)
            return;

        if (_shieldActive)
        {
            DeactivateShield();
            return;
        }

        _health -= damage;
        if (_health < 1)
        {
            Death();
        }
    }

    void Death()
    {
        _enemyMovement.StopMoving();

        _animStateChanger.PlayDeathAnimation();

        GameManager.Instance.IncrementScore();
        StartCoroutine(WaitForDeathAnimToFinishRtn());
        AudioManager.Instance.PlayEnemyDeath();
    }
    IEnumerator WaitForDeathAnimToFinishRtn()
    {
        float clipLength = _animStateChanger.DeathAnimClipLength();
        yield return HM.WaitTime(clipLength);
        Destroy(_enemyContainer);
    }

    /// <summary>
    /// Adds a shield, after wave 2, to the enemy based on random chance. 
    /// This shield give the enemy 1 extra hit.
    /// </summary>
    void RollForShield()
    {
        int wave = SpawnManager.Instance.CurrentWave;
        if (wave >= 3)
        {
            //determine how many waves to calculate odds for
            float firstWave = 3;
            float lastWave = 10;
            float totalShieldWaves = lastWave - firstWave;
            
            //determine how many percentage points to increment between (the range)
            float minOdds = 10;
            float maxOdds = 40;
            float range = maxOdds - minOdds;

            //the percentage increase for each successive wave
            float increment = range / totalShieldWaves;
            
            ///<summary> Formula
            /// This formula is the chance that needs to be met for shields to activate.
            /// Odds increase each wave between shield waves.
            /// Has min and max odds
            /// 
            /// minOdds[10] + (wave - firstWave[3]) * increment[4.286]
            ///</summary>
            float odds = minOdds + (wave - firstWave) * increment;
            
            //cap odds at max value
            if (odds > maxOdds) odds = maxOdds;

            int rand = Random.Range(1, 101);

            if (rand <= odds)
            {
                ActivateShield();
            }
        }
    }

    void ActivateShield()
    {
        _shieldActive = true;
        _shieldGO.SetActive(true);
    }
    void DeactivateShield()
    {
        _shieldActive = false;
        _shieldGO.SetActive(false);
    }


}