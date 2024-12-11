#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles moving the Enemy
/// </summary>
public abstract class EnemyMovement : MonoBehaviour 
{
#region Variables

    protected float _moveSpeed;
    [SerializeField] protected float _baseMoveSpeed = 4;
    [SerializeField] protected float _yOffScreenPoint, _yResetPoint, _xBounds;

#endregion
#region Base Methods

    protected virtual void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }
	protected virtual void Update() 
    {
        DownwardMovement();
	}

    #endregion

    /// <summary>
    /// Moves the enemy to its starting location when it spawns.
    /// </summary>
    protected virtual void MoveToSpawnPosition()
    {
        transform.position = new Vector2(Random.Range(_xBounds * -1, _xBounds), _yResetPoint);
    }
    
    protected virtual void DownwardMovement()
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime, Space.World);
        ResetPosition();
    }

    protected virtual void ResetPosition()
    {
        if (transform.position.y < _yOffScreenPoint)
        {
            var rand = Random.Range(_xBounds * -1, _xBounds);
            transform.position = new Vector2(rand, _yResetPoint);
        }
    }

    public virtual void StopMoving()
    {
        _moveSpeed = 0;
    }

}