#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Zombie : MonoBehaviour 
{
#region Variables

    enum State
    {
        Spawning,
        Chasing,
        Attacking,
        Dying
    }
    State _state = State.Spawning;

    [SerializeField] GameObject _heart;

    Vector2 _playerPos;
    [SerializeField] float _attackRange = 0.2f;
    [SerializeField] float _atkCooldown = 1.1f;
    float _cooldownTimer;
    [SerializeField] Animator _anim;

    [SerializeField] float _speedChangeFactor = 0.5f;
    [SerializeField] float _baseMoveSpeed = 1;
    float _curMoveSpeed;
    /// <summary>
    /// Slows move speed until it's 0.
    /// </summary>
    void SlowDown()
    {
        if (_curMoveSpeed != 0)
        {
            _curMoveSpeed = Mathf.Lerp(_curMoveSpeed, 0, _speedChangeFactor * Time.deltaTime);

            //if close enough, set to 0
            if (_curMoveSpeed <= 0.05f)
                _curMoveSpeed = 0;
        }
    }
    /// <summary>
    /// Accelerates move speed until it's back to normal.
    /// </summary>
    void Accelerate()
    {
        if (_curMoveSpeed != _baseMoveSpeed)
        {
            _curMoveSpeed = Mathf.Lerp(_curMoveSpeed, _baseMoveSpeed, _speedChangeFactor * Time.deltaTime);

            //if close enough, set to base
            if (Mathf.Abs(_curMoveSpeed - _baseMoveSpeed) < 0.05f)
                _curMoveSpeed = _baseMoveSpeed;
        }
    }

#endregion
#region Base Methods

    void Start() 
    {
        RandomizeAnimStartDirection();
	}
	
	void Update() 
    {
        _playerPos = GameManager.Instance.PlayerPosition();

		if (_state == State.Chasing)
        {
            Accelerate();
            Chase();
        }
        else if (_state == State.Attacking)
        {
            SlowDown();
            Attack();
        }
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //Visualize the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
#endif

#endregion

    /// <summary>
    /// Randomize the animation start direction to left, right, up, or down.
    /// </summary>
    void RandomizeAnimStartDirection()
    {
        int rand = Random.Range(0, 4);
        Vector2 dir;

        switch (rand)
        {
            case 1:
                dir = Vector2.left;
                break;
            case 2:
                dir = Vector2.right;
                break;
            case 3:
                dir = Vector2.up;
                break;
            default:
                dir = Vector2.down;
                break;
        }

        _anim.SetFloat("Horizontal", dir.x);
        _anim.SetFloat("Vertical", dir.y);
    }

    public void FinishedSpawning()
    {
        _state = State.Chasing;
        _heart.SetActive(true);
    }

    void Chase()
    {
        //move towards target at speed
        Vector2 dirToPlayer = _playerPos - (Vector2)transform.position;
        dirToPlayer.Normalize();

        float moveSpeed = _curMoveSpeed * Time.deltaTime;
        transform.Translate(dirToPlayer * moveSpeed);

        ChangeAnimDirection(dirToPlayer);

        if (PlayerInAttackRange())
            _state = State.Attacking;   
    }

    void Attack()
    {
        if (!PlayerInAttackRange())
        {
            _anim.SetBool("waitingToAttack", false);
            _state = State.Chasing;
            return;
        }
        
        if (CooldownFinished())
        {
            _cooldownTimer = Time.time + _atkCooldown;
            _anim.SetTrigger("attack");
        }
        else
        {
            _anim.SetBool("waitingToAttack", true);
        }
    }
    bool CooldownFinished()
    {
        if (_cooldownTimer < Time.time)
            return true;
        return false;
    }

    bool PlayerInAttackRange()
    {
        if (Vector2.Distance(transform.position, _playerPos) <= _attackRange)
            return true;
        return false;
    }

    /// <summary>
    /// Changes the animation direction parameters to play animations that face in the player's direction.
    /// </summary>
    /// <param name="dirToPlayer"></param>
    void ChangeAnimDirection(Vector2 dirToPlayer)
    {
        _anim.SetFloat("Horizontal", dirToPlayer.x);
        _anim.SetFloat("Vertical", dirToPlayer.y);
    }


}