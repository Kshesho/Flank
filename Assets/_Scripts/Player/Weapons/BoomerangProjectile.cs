#region Using Statements
using Narzioth.Utilities;
using UnityEngine;
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
        InitialMovement,
        RotatingTowardsTarget,
        FacingTarget,
        RotatingTowardsPlayer,
        FacingPlayer
    }
    State _moveState;

    [SerializeField] float _rotSpeed = 4;

    Transform _target, _defaultTarget;
    Vector2 _initialTargetPos;
    float _targetReachedValue = 0.2f;
    bool _wasEnemyHit;

#endregion
#region Base Methods

    void Start()
    {
        _defaultTarget = GameObject.Find("Default Boomerang target").transform;
        _initialTargetPos = new Vector2(transform.position.x, transform.position.y + 1.5f);
        _moveState = State.InitialMovement;
    }

    void OnEnable()
    {
        AudioManager.Instance.Play_BoomerangFly();
    }
    void OnDisable()
    {
        AudioManager.Instance.Stop_BoomerangFly();
    }

    void FixedUpdate()
    {
        switch (_moveState)
        {
            case State.InitialMovement:
                if (_defaultTarget == null)
                {
                    _moveState = State.RotatingTowardsPlayer;
                }
                else InitialForwardMovement();
                break;
            //------------------------------
            case State.RotatingTowardsTarget:
                if (_target == null)
                {
                    _moveState = State.RotatingTowardsPlayer;
                }
                else MoveTowardsTarget_Rotating();
                break;
            //------------------------------
            case State.FacingTarget:
                if (_target == null)
                {
                    _moveState = State.RotatingTowardsPlayer;
                }
                else MoveTowardsTarget_Facing();
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

#endregion

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        //should damage enemies on the way to target and back to player
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, _damage);
            AudioManager.Instance.PlayEnemyImpact();

            _wasEnemyHit = true;
        }
        else if (other.CompareTag(Tags.Boss))
        {
            Events.OnBossCollide_Projectile(_damage, this.gameObject);

            _wasEnemyHit = true;
        }
    }

    #region Movement

    /// <summary>
    /// Step 1. 
    /// Move straight, simulating initial throw acceleration.
    /// </summary>
    void InitialForwardMovement()
    {
        //once +1.5 on the Y OR default target reached, find target
        ///Why check for both targets?
        /// We want to make sure we account for reaching the default target first, 
        /// since it doesn't go past the screen bounds but the initial target can.
        /// If the the initial target is past the default target and I move to it,
        /// I will then try and move to the default target and may get stuck.
        if (Vector2.Distance(transform.position, _initialTargetPos) <= _targetReachedValue ||
            Vector2.Distance(transform.position, _defaultTarget.position) <= _targetReachedValue)
        {
            FindTarget();
            _moveState = State.RotatingTowardsTarget;
        }
    }

    /// <summary>
    /// Step 2. Rotate towards target.
    /// </summary>
    void MoveTowardsTarget_Rotating()
    {
        if (transform.LookAt2D(_target, _rotSpeed) //once the ratation faces the target
            || //or target reached before rotation reached
            Vector2.Distance(transform.position, _target.position) <= _targetReachedValue) 
        {
            _moveState = State.FacingTarget;
        }
    }

    /// <summary>
    /// Step 3. Lock direction towards target.
    /// </summary>
    void MoveTowardsTarget_Facing()
    {
        transform.LookAt2D(_target);
        if (Vector2.Distance(transform.position, _target.position) <= _targetReachedValue)
            _moveState = State.RotatingTowardsPlayer;
    }

    /// <summary>
    /// Step 4. Rotate back to face player.
    /// </summary>
    void MoveTowardsPlayer_Rotating()
    {
        var playerTrans = GameManager.Instance.PlayerTransform();
        if (transform.LookAt2D(playerTrans, _rotSpeed)//once the ratation faces the target
            || //or target reached before rotation reached
            Vector2.Distance(transform.position, playerTrans.position) <= _targetReachedValue)
        {
            _moveState = State.FacingPlayer;
        }
    }

    /// <summary>
    /// Step 5. Lock direction towards player.
    /// </summary>
    void MoveTowardsPlayer_Facing()
    {
        var playerTrans = GameManager.Instance.PlayerTransform();
        transform.LookAt2D(playerTrans);
        if (Vector2.Distance(transform.position, playerTrans.position) <= _targetReachedValue)
        {
            Events.OnBoomerangReturned?.Invoke(_wasEnemyHit);
            Destroy(this.gameObject);
            // v use this to loop boomerang movement instead v
            //_moveState = State.RotatingTowardsTarget;
        }
    }

    #endregion

    void FindTarget()
    {
        //cast a box ray the size of the play area 
        Vector2 boxSize = new Vector2((10.15f * 2), (5.4f * 2));
        RaycastHit2D[] hits = Physics2D.BoxCastAll(Vector2.zero, boxSize, 0, Vector2.zero, 0);
        
        foreach(var hit in hits)
        {
            var col = hit.collider;
            //if enemy hit
            if (col.CompareTag(Tags.EDamagable) || col.CompareTag(Tags.Boss))
            {
                if (_target == null)
                    _target = hit.transform;
                else
                {
                    var distanceToPreviousTarget = Vector2.Distance(transform.position, _target.position);
                    var distanceToNewTarget = Vector2.Distance(transform.position, hit.transform.position);
                    if (distanceToNewTarget < distanceToPreviousTarget)
                    {
                        _target = hit.transform;
                    }
                }
            }
        }

        //if no target found, revert to default target
        if (_target == null)
            _target = _defaultTarget;
    }

}