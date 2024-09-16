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

    float _xInput, _yInput;
    Vector2 _curMoveDirection = Vector2.zero;
    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;
    [SerializeField] Vector2 _startPos;
    [SerializeField] float _normalMoveSpeed = 3f;
    float _curMoveSpeed;

    //----Dodge
    float _dodgeMoveSpeed;
    bool _dodging;
    float _canDodgeTime = -1; 
    /// <summary>
    /// Returns false if stamina is too low or dodge is still happening.
    /// </summary>
    /// <returns></returns>
    bool CanDodge()
    {
        if (_canDodgeTime > Time.time)
            return false;
        if (CurStamina < _dodgeStaminaCost)
            return false;
        
        return true;
    }
    [SerializeField] float _dodgeDuration = 0.3f, _dodgeSpeedMultiplier = 1.1f;
    [SerializeField] float _dodgeCooldown = 0.3f;
    Coroutine _resetDodgeCooldownRtn;
    [SerializeField] float _dodgeCooldownPowerupActiveTime = 5;
    
    //----Stamina
    float _curStamina_DONTALTER;
    float CurStamina 
    { 
        get
        {
            return _curStamina_DONTALTER;
        }
        set
        {
            _curStamina_DONTALTER = value;
            if (_curStamina_DONTALTER < 0)
                _curStamina_DONTALTER = 0;
            else if (_curStamina_DONTALTER > _maxStamina)
                _curStamina_DONTALTER = _maxStamina;
        }
    } 
    /// <summary>
    /// Takes away dodge stamina cost from Stamina and updates UI.
    /// </summary>
    void DecrementStamina()
    {
        CurStamina -= _dodgeStaminaCost;
        UIManager.Instance.SetStaminaBarFill(CurStamina / _maxStamina);
    }
    float _dodgeStaminaCost = 0.35f;
    float _maxStamina = 1;
    float _defStaminaGain = 0.25f, _powerStaminaGain = 0.4f;
    float _staminaGainPerSecond;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnPowerupCollected += CollectDodgePowerup;
    }
    void OnDisable()
    {
        Events.OnPowerupCollected -= CollectDodgePowerup;
    }

    void Start() 
    {
        _curMoveSpeed = _normalMoveSpeed;
        _staminaGainPerSecond = _defStaminaGain;
        transform.position = _startPos;
        CurStamina = _maxStamina;
	}

    void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        _dodgeMoveSpeed = _normalMoveSpeed * _dodgeSpeedMultiplier;
        CheckForDodgeInput();
        RefillStamina();
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

    #region Movement

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

    #endregion

    #region Dodge

    void CheckForDodgeInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (CanDodge())
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
        DecrementStamina();
    }
    
    /// <summary>
    /// Sets the values outside this class that are part of the dodge mechanic.
    /// </summary>
    void SetDodgeForeignValues()
    {
        _animStateChanger.DodgeStarted();
        transform.localScale = Vector2.one * 1.1f;
        _heart.EnableDodgeInvulnerability();            
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
        _heart.DisableDodgeInvulnerability();
    }

    public void CollectDodgePowerup(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.DodgeCooldown)
        {
            _staminaGainPerSecond = _powerStaminaGain;
            if (_resetDodgeCooldownRtn != null) 
            { 
                StopCoroutine(_resetDodgeCooldownRtn); 
            }
            _resetDodgeCooldownRtn = StartCoroutine(ResetDodgeCooldownRtn());
        }
    }
    IEnumerator ResetDodgeCooldownRtn()
    {
        yield return HM.WaitTime(_dodgeCooldownPowerupActiveTime);
        _staminaGainPerSecond = _defStaminaGain;
    }

    /// <summary>
    /// Refills the stamina amount at a constant rate and updates the UI as well.
    /// </summary>
    void RefillStamina()
    {
        if (CurStamina >= _maxStamina)
            return;

        CurStamina += _staminaGainPerSecond * Time.deltaTime;
        //the 0-1 ratio (fill amount)
        float curStaminaRatio = CurStamina / _maxStamina;

        UIManager.Instance.SetStaminaBarFill(curStaminaRatio);
    }

    #endregion


}