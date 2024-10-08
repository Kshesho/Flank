#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime;
using UnityEngine;
#endregion

/// <summary>
/// Handles player movement 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
#region Variables

    //--references
    [SerializeField] Rigidbody2D _rBody;
    [SerializeField] PlayerAnimStateChanger _animStateChanger;
    [SerializeField] PlayerHeart _heart;

    //----Movement
    float _xInput, _yInput;
    Vector2 _curMoveDirection = Vector2.zero;
    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;
    [SerializeField] Vector2 _startPos;
    [SerializeField] float _walkSpeed = 100, _sprintSpeed = 200, _dodgeMoveSpeed;
    float _curSpeed;
    bool _sprinting;

    //----Dodge
    bool _dodging;
    float _canDodgeTime = -1;

    [SerializeField] float _dodgeDuration = 0.3f, _dodgeSpeedMultiplier = 1.1f;
    [SerializeField] float _dodgeCooldown = 0.3f;
    Coroutine _resetDodgeCooldownRtn;
    [SerializeField] float _dodgeCooldownPowerupActiveTime = 5;

    //----Stamina
    bool _staminaOnCooldown;
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
    float _dodgeStaminaCost = 0.35f, _sprintStaminaCostPerSec = 0.25f;
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
        _curSpeed = _walkSpeed;
        _staminaGainPerSecond = _defStaminaGain;
        transform.position = _startPos;
        CurStamina = _maxStamina;
    }

    void Update()
    {
        if (GameManager.Instance.GamePaused)
            return;

        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        _dodgeMoveSpeed = _sprintSpeed * _dodgeSpeedMultiplier;
        CheckForSprintInput();
        CheckForDodgeInput();

        UpdateStamina();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GamePaused)
            return;

        if (_dodging)
        {
            _rBody.velocity = _curMoveDirection * _curSpeed * Time.fixedDeltaTime;
        }
        else Move();

        ConstrainPosition();
    }

#endregion

    #region Movement

    /// <summary>
    /// Moves the player at walk or run speed, based on the current move speed.
    /// </summary>
    void Move()
    {
        _curMoveDirection.x = _xInput;
        _curMoveDirection.y = _yInput;
        _curMoveDirection.Normalize();
        _rBody.velocity = _curMoveDirection * _curSpeed * Time.fixedDeltaTime;
    }

    void ConstrainPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, (_xBounds * -1), _xBounds);
        float clampedY = Mathf.Clamp(transform.position.y, ((_yBounds + 0.65f) * -1), _yBounds);
        transform.position = new Vector2(clampedX, clampedY);
    }

    void CheckForSprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (CanSprint())
            {
                StartSprinting();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSprinting();
        }
    }
    void StartSprinting()
    {
        _sprinting = true;
        _animStateChanger.SprintStarted();
        if (!_dodging)
            _curSpeed = _sprintSpeed;
    }
    void StopSprinting()
    {
        _sprinting = false;
        _animStateChanger.SprintStopped();
        if (!_dodging)
        {
            _curSpeed = _walkSpeed;
        }
    }

    #endregion

    #region Dodge

    void CheckForDodgeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        _curSpeed = _dodgeMoveSpeed;
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

        if (_sprinting)
            _curSpeed = _sprintSpeed;
        else
            _curSpeed = _walkSpeed;

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



    #endregion

    #region Stamina

    void UpdateStamina()
    {
        if (_sprinting)
        {
            DrainStamina();
        }
        else if (!_staminaOnCooldown)
        {
            RefillStamina();
        }
    }

    /// <summary>
    /// Takes away dodge stamina cost from Stamina and updates UI.
    /// </summary>
    void DecrementStamina()
    {
        CurStamina -= _dodgeStaminaCost;
        CheckIfStaminaEmpty();
        UpdateStaminaUI();
    }

    /// <summary>
    /// Takes away stamina per second while sprinting, and updates the UI.
    /// </summary>
    void DrainStamina()
    {
        CurStamina -= _sprintStaminaCostPerSec * Time.deltaTime;
        CheckIfStaminaEmpty();
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        // TODO: if this is being changed every frame anyway, is there a better way to update this in the UI manager?
        UIManager.Instance.SetStaminaBarFill(CurStamina / _maxStamina);
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

    void CheckIfStaminaEmpty()
    {
        if (CurStamina <= 0)
        {
            _staminaOnCooldown = true;
            StopSprinting();
            UIManager.Instance.StaminaCooldownVisual_On();
            StartCoroutine(StaminaCooldownRtn());
        }
    }
    IEnumerator StaminaCooldownRtn()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        yield return HM.WaitTime(2);
        
        while (CurStamina < _maxStamina)
        {
            RefillStamina();
            yield return waitForEndOfFrame;
        }

        UIManager.Instance.StaminaCooldownVisual_Off();
        _staminaOnCooldown = false;
    }

    #endregion

    #region return types

    /// <summary>
    /// Returns false if dodge is on cooldown, stamina on cooldown, stamina is too low, player isn't moving.
    /// </summary>
    /// <returns></returns>
    bool CanDodge()
    {
        if (_canDodgeTime > Time.time)
            return false;
        if (_staminaOnCooldown || CurStamina < _dodgeStaminaCost)
            return false;
        if (!PlayerIsMoving())
            return false;
        
        return true;
    }

    /// <summary>
    /// Returns false if stamina is on cooldown or player is already sprinting.
    /// </summary>
    /// <returns></returns>
    bool CanSprint()
    {
        if (_sprinting)
            return false;
        if (_staminaOnCooldown)
            return false;

        return true;
    }

    bool PlayerIsMoving()
    {
        if (_xInput == 0 && _yInput == 0)
            return false;

        return true;
    }

    #endregion


}