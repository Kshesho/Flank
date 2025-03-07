#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Holds and changes Zombie's state and acts as the Zombie controller.
/// </summary>
public class Zombie : MonoBehaviour 
{
#region Variables

    [SerializeField] GameObject _heart;
    [SerializeField] ZombieAnimStateChanger _animStateChanger;

    public ZombieState State { get; private set; } = ZombieState.Spawning;

    [SerializeField] float _attackRange = 0.8f;
    Vector2 _playerPos;

#endregion
#region Base Methods

    void Update() 
    {
        _playerPos = GameManager.Instance.PlayerPosition();

        if (State == ZombieState.Chasing)
        {
            if (PlayerInAttackRange())
            {
                State = ZombieState.Attacking;
            }
        }
        else if (State == ZombieState.Attacking)
        {
            if (!PlayerInAttackRange())
            {
                _animStateChanger.Stop_WaitingToAttack();
                State = ZombieState.Chasing;
            }   
        }
        else if (State == ZombieState.Dying)
        {
            //make sure weapon is disabled?
        }
	}

#if UNITY_EDITOR
    [SerializeField] bool _visualizeAttackRange = true;

    void OnDrawGizmos()
    {
        if (_visualizeAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
#endif

#endregion

    public void FinishedSpawning()
    {
        State = ZombieState.Chasing;
        _heart.SetActive(true);
    }
    
    bool PlayerInAttackRange()
    {
        if (Vector2.Distance(transform.position, _playerPos) <= _attackRange)
            return true;
        return false;
    }

}