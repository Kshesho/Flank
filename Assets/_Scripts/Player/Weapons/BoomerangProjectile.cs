#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;
using static UnityEngine.GraphicsBuffer;
#endregion

/// <summary>
/// Handles movement and collision for boomerang projectile.
/// </summary>
public class BoomerangProjectile : Projectile
{
#region Variables

    enum State
    {
        None,
        RotatingTowardsTarget,
        FacingTarget,
        RotatingTowardsPlayer,
        FacingPlayer
    }
    State _moveState;
    [SerializeField] float _initialForce = 10, _rotSpeed = 4;
    [SerializeField] Transform _target;
    float _targetReachedValue = 0.2f;

#endregion
#region Base Methods

    void OnEnable()
    {
        //_rBody.AddForce(transform.up * _initialForce, ForceMode2D.Impulse);
        FindTarget();
        _moveState = State.RotatingTowardsTarget;
    }
    void OnDisable()
    {
        
    }

    protected override void Update()
    {}

    void FixedUpdate()
    {
        switch (_moveState)
        {
            case State.RotatingTowardsTarget:
                MoveTowardsTarget_Rotating();
                break;
            //------------------------------
            case State.FacingTarget:
                MoveTowardsTarget_Facing();
                break;
            //------------------------------
            case State.RotatingTowardsPlayer:
                MoveTowardsPlayer_Rotating();
                break;
            //------------------------------
            case State.FacingPlayer:
                MoveTowardsPlayer_Facing();
                break;
        }

        transform.Translate(Vector2.up * _moveSpeed * Time.fixedDeltaTime, Space.Self);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //should damage enemies on the way to target and back to player
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, _damage);
            AudioManager.Instance.PlayEnemyImpact();
        }
    }

#endregion

    void MoveTowardsTarget_Rotating()
    {
        if (transform.LookAt2D(_target, _rotSpeed) //once the ratation faces the target
            || //or target reached before rotation reached
            Vector2.Distance(transform.position, _target.position) < _targetReachedValue) 
        {
            _moveState = State.FacingTarget;
        }
    }

    void MoveTowardsTarget_Facing()
    {
        transform.LookAt2D(_target);
        if (Vector2.Distance(transform.position, _target.position) < _targetReachedValue)
            _moveState = State.RotatingTowardsPlayer;
    }

    void MoveTowardsPlayer_Rotating()
    {
        var playerTrans = GameManager.Instance.PlayerTransform();
        if (transform.LookAt2D(playerTrans, _rotSpeed)//once the ratation faces the target
            || //or target reached before rotation reached
            Vector2.Distance(transform.position, playerTrans.position) < _targetReachedValue)
        {
            _moveState = State.FacingPlayer;
        }
    }

    void MoveTowardsPlayer_Facing()
    {
        var playerTrans = GameManager.Instance.PlayerTransform();
        transform.LookAt2D(playerTrans);
        if (Vector2.Distance(transform.position, playerTrans.position) < _targetReachedValue)
        {
            Events.OnBoomerangReturned?.Invoke();
            Destroy(this.gameObject);
            // v use this to loop boomerang movement instead v
            //_moveState = State.RotatingTowardsTarget;
        }
    }

    void FindTarget()
    {
        _target = GameObject.Find("Boomerang target").transform;
        //cast a box ray the size of the play area
        //compile list of targets
        //move towards the closest target (in front of player?)
        //if none found, fly forward a little, then return to player
    }

}