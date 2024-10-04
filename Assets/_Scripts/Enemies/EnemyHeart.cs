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

#endregion
#region Base Methods

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


}