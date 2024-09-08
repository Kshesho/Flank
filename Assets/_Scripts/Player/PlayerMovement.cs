#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles player movement 
/// </summary>
public class PlayerMovement : MonoBehaviour 
{
#region Variables

    [SerializeField] Rigidbody2D _rBody;
    [SerializeField] PlayerAnimStateChanger _animStateChanger;
    [SerializeField] PlayerHeart _heart;

    [SerializeField] Vector2 _startPos;
    [SerializeField] float _normalMoveSpeed = 3f;
    float _curMoveSpeed, _dodgeMoveSpeed;
    float _canDodgeTime = -1; 
    bool DodgeCooldownFinished()
    {
        if (_canDodgeTime < Time.time)
            return true;
        return false;
    }
    [SerializeField] float _dodgeDuration = 0.3f, _dodgeCooldown = 2f, _dodgeSpeedMultiplier = 1.1f;
    bool _dodging;

    float _xInput, _yInput;
    Vector2 _curMoveDirection = Vector2.zero;

    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;

#endregion
#region Base Methods

    void Start() 
    {
        _curMoveSpeed = _normalMoveSpeed;
        transform.position = _startPos;
	}

    void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        _dodgeMoveSpeed = _normalMoveSpeed * _dodgeSpeedMultiplier;
        CheckForDodgeInput();
    }

    void FixedUpdate() 
    {
        if (_dodging)
        {
            _rBody.velocity = _curMoveDirection * _curMoveSpeed * Time.fixedDeltaTime;
        }
        else Move();
        
        ConstrainPosition();
	}

#endregion

    void Move()
    {
        //Vector2 dir = new Vector2(_xInput, _yInput);
        _curMoveDirection.x = _xInput;
        _curMoveDirection.y = _yInput;
        _curMoveDirection.Normalize();
        //transform.Translate(dir * _moveSpeed * Time.fixedDeltaTime);
        _rBody.velocity = _curMoveDirection * _curMoveSpeed * Time.fixedDeltaTime;
    }

    void ConstrainPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, (_xBounds * -1), _xBounds);
        float clampedY = Mathf.Clamp(transform.position.y, ((_yBounds + 0.65f) * -1), _yBounds);
        transform.position = new Vector2(clampedX, clampedY);
    }

    #region Dodge

    void CheckForDodgeInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (DodgeCooldownFinished())
            {
                Dodge();
            }
        }
    }
    void Dodge()
    {
        SetDodgeLocalValues();
        SetDodgeForeignValues();
        StartCoroutine(EndDodgeRtn());
    }
    /// <summary>
    /// Sets the values in this class that are needed for dodging.
    /// </summary>
    void SetDodgeLocalValues()
    {
        _canDodgeTime = Time.time + _dodgeCooldown;
        _dodging = true;
        _curMoveSpeed = _dodgeMoveSpeed;
    }
    /// <summary>
    /// Sets the values outside this class that are part of the dodge mechanic.
    /// </summary>
    void SetDodgeForeignValues()
    {
        _animStateChanger.DodgeStarted();
        transform.localScale = Vector2.one * 1.1f;
        _heart.EnableDeathless();            
    }
    /// <summary>
    /// Ends the dodge after dodge duration.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndDodgeRtn()
    {
        yield return HM.WaitTime(_dodgeDuration);
        _dodging = false;
        _curMoveSpeed = _normalMoveSpeed;
        _animStateChanger.DodgeFinished();
        transform.localScale = Vector2.one;
        _heart.DisableDeathless();
    }

    #endregion


}