#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles moving the Enemy
/// </summary>
public class EnemyMovement : MonoBehaviour 
{
    #region Variables

    [SerializeField] EnemyAnimStateChanger _animStateChanger;

    [SerializeField] float _moveSpeed = 4;
    [SerializeField] float _yOffScreenPoint, _yResetPoint, _xBounds;

    int _diagonalMovementChance = 10;
    bool _moveDiagonally;
    Vector2 _curMoveDirection;
    Vector2 _sW = new Vector2(-1, -1);//Direction 1
    Vector2 _sE = new Vector2(1, -1);//Direction 2

#endregion
#region Base Methods
	
    void Start()
    {
        RollForDiagonalMovevment();
    }

	void Update () 
    {
        if (_moveDiagonally)
        {
            DiagonalMovement();
        }
        else DownwardMovement();
	}

#endregion

    void DownwardMovement()
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime, Space.World);

        if (transform.position.y < _yOffScreenPoint)
        {
            var rand = Random.Range(_xBounds * -1, _xBounds);
            transform.position = new Vector2(rand, _yResetPoint);
        }
    }

    /// <summary>
    /// There is a 10% chance that this enemy will move diagonally.
    /// </summary>
    void RollForDiagonalMovevment()
    {
        //roll random number from 1-100
        int rand = Random.Range(1, 101);
        if (rand <= _diagonalMovementChance)
        {
            _moveDiagonally = true;
            //lower movement speed due to increased diagonal move speed
            _moveSpeed *= 0.75f;
            //pick a move direction based on if I spawned to the left or right of 0
            if (transform.position.x < 0)
            {
                _curMoveDirection = _sE;
                _animStateChanger.ChageRunDirection(1);
            }
            else
            {
                _curMoveDirection = _sW;
                _animStateChanger.ChageRunDirection(-1);
            }
        }
    }
    void DiagonalMovement()
    {
        //move in current direction
        transform.Translate(_curMoveDirection * _moveSpeed * Time.deltaTime, Space.World);
        
        //if I hit the edge of the left or right side of the screen, move the other way
        if (transform.position.x > _xBounds ||
            transform.position.x < _xBounds * -1)
        {
            ChangeHorizontalMoveDirection();
        }

        //when I reset to the top of the screen, don't randomize my X pos
        if (transform.position.y < _yOffScreenPoint)
            transform.position = new Vector2(transform.position.x, _yResetPoint);
    }
    void ChangeHorizontalMoveDirection()
    {
        if (_curMoveDirection == _sW)
        {
            _curMoveDirection = _sE;
            _animStateChanger.ChageRunDirection(1);
        }
        else if (_curMoveDirection == _sE)
        {
            _curMoveDirection = _sW;
            _animStateChanger.ChageRunDirection(-1);
        }
        else Debug.LogError("Invalid Enemy horizontal move direction!");
    }

    public void StopMoving()
    {
        _moveSpeed = 0;
    }


}