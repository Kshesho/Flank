#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class ZombieMovement : EnemyMovement 
{
#region Variables

    [SerializeField] Zombie _zombie;
    [SerializeField] ZombieAnimStateChanger _animStateChanger;
    Vector2 _playerPos;
    [SerializeField] float _speedChangeFactor = 1.5f;

    bool _stopMovement;

#endregion
#region Base Methods
	
	protected override void Update() 
    {
        if (_stopMovement) 
            return;

		_playerPos = GameManager.Instance.PlayerPosition();

        if (_zombie.State == ZombieState.Chasing)
        {
            //Accelerate();
            Chase();
        }
        else if (_zombie.State == ZombieState.Attacking)
        {
            //SlowDown();
        }
	}

#endregion

    /// <summary>
    /// Accelerates move speed until it's back to normal.
    /// </summary>
    void Accelerate()
    {
        if (_moveSpeed != _baseMoveSpeed)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, _baseMoveSpeed, _speedChangeFactor * Time.deltaTime);

            //if close enough, set to base
            if (Mathf.Abs(_moveSpeed - _baseMoveSpeed) < 0.05f)
                _moveSpeed = _baseMoveSpeed;
        }
    }
    /// <summary>
    /// Slows move speed until it's 0.
    /// </summary>
    void SlowDown()
    {
        if (_moveSpeed != 0)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, 0, _speedChangeFactor * Time.deltaTime);

            //if close enough, set to 0
            if (_moveSpeed <= 0.05f)
                _moveSpeed = 0;
        }
    }

    void Chase()
    {
        //move towards target at speed
        Vector2 dirToPlayer = _playerPos - (Vector2)transform.position;
        dirToPlayer.Normalize();

        float moveSpeed = _moveSpeed * Time.deltaTime;
        transform.Translate(dirToPlayer * moveSpeed);

        _animStateChanger.ChangeAnimDirection(dirToPlayer);
    }

    public override void StopMoving()
    {
        _stopMovement = true;
        base.StopMoving();
    }

}