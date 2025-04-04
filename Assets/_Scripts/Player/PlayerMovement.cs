#region Using Statements
using Narzioth.Utilities;
using System.Collections;
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
    [SerializeField] PlayerHeart _heart;
    [SerializeField] SpriteRenderer _playerSpriteRend;
    
    //----Movement
    float _xInput, _yInput;
    Vector2 _curMoveDirection = Vector2.zero;
    [Space(15)]
    [SerializeField] Vector2 _startPos;
    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;
    [SerializeField] float _walkSpeed = 100, _sprintSpeed = 200, _heavyAtkMoveSpeed = 50;
    float _curSpeed;
    bool _sprinting;

    [SerializeField] GameObject _net;
    Coroutine _resumeMovementRtn;
    
    //speed is multiplied by this to slow the player when slow powerup is collected
    float _slowedMovementPrct = 1f;
    Coroutine _removeSlowRtn;
    float _slowDuration = 3f;

    //----Dodge
    bool _dodging;
    float _canDodgeTime = -1;
    Color _fadedAlpha = new Color(1, 1, 1, 0.3f);

    [Space(15)]
    [SerializeField] float _dodgeSpeed = 220;
    [SerializeField] float _dodgeDuration = 0.3f;
    [SerializeField] float _dodgeCooldown = 0.3f;

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
    [SerializeField][Range(0f, 1f)] float _dodgeStaminaCost = 0.35f, _sprintStaminaCostPerSec = 0.25f;
    float _maxStamina = 1;
    float _defStaminaGain = 0.3f;
    float _staminaGainPerSecond;
    //Stamina Gain powerup
    float _boostedStaminaGain = 0.85f;
    Coroutine _disableStaminaBoostRtn;
    [SerializeField] float _staminaBoostPowerupActiveTime = 5;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnPowerupCollected += CollectStaminaBoostPowerup;
        Events.OnPowerupCollected += CollectSlowPowerup;
        Events.OnPlayerHeavyAttackStarted += SetHeavyAttackSpeed;
        Events.OnPlayerHeavyAttackFinished += ResetSpeed;
        Events.OnPlayerNetted += HaltMovement;
        Events.OnPlayerDeath += StopMovement;
    }
    void OnDisable()
    {
        Events.OnPowerupCollected -= CollectStaminaBoostPowerup;
        Events.OnPowerupCollected -= CollectSlowPowerup;
        Events.OnPlayerHeavyAttackStarted -= SetHeavyAttackSpeed;
        Events.OnPlayerHeavyAttackFinished -= ResetSpeed;
        Events.OnPlayerNetted -= HaltMovement;
        Events.OnPlayerDeath -= StopMovement;
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

        HandleMovementInput();

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
            Move_LockedDirection();
        }
        else Move();

        ConstrainPosition();
    }

#endregion

    #region Movement

    void HandleMovementInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if (_xInput != 0 || _yInput != 0)
            PlayerStateManager.Instance.MovementStarted();
        else
            PlayerStateManager.Instance.MovementStopped();
        
    }

    /// <summary>
    /// Moves the player at walk or run speed, based on the current move speed.
    /// </summary>
    void Move()
    {
        _curMoveDirection.x = _xInput;
        _curMoveDirection.y = _yInput;
        _curMoveDirection.Normalize();
        _rBody.velocity = _curMoveDirection * (_curSpeed * _slowedMovementPrct) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Moves the player at current speed without changing the direction.
    /// </summary>
    void Move_LockedDirection()
    {
        _rBody.velocity = _curMoveDirection * (_curSpeed * _slowedMovementPrct) * Time.fixedDeltaTime;
    }

    void ConstrainPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, (_xBounds * -1), _xBounds);
        float clampedY = Mathf.Clamp(transform.position.y, ((_yBounds + 0.65f) * -1), _yBounds);
        transform.position = new Vector2(clampedX, clampedY);
    }

    void CheckForSprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (CanSprint())
            {
                StartSprinting();
            }
        }
        else if (_sprinting)
        {
            //only stop sprinting if both keys are NOT being held
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                StopSprinting();
            }
        }
    }
    void StartSprinting()
    {
        _sprinting = true;
        PlayerStateManager.Instance.SprintStarted();

        if (!_dodging)
            _curSpeed = _sprintSpeed;
    }
    void StopSprinting()
    {
        _sprinting = false;
        PlayerStateManager.Instance.SprintStopped();
        ResetSpeed();
    }

    /// <summary>
    /// Stops the player's movement, but keeps this component active.
    /// </summary>
    void HaltMovement()
    {
        _rBody.velocity = Vector2.zero;
        _curSpeed = 0;
        _net.SetActive(true);
        UIManager.Instance.EnableStoppedIcon();

        if (_resumeMovementRtn != null) StopCoroutine(_resumeMovementRtn);
        _resumeMovementRtn = StartCoroutine(ResumeMovementRtn());
    }
    IEnumerator ResumeMovementRtn()
    {
        yield return HM.WaitTime(2);
        PlayerStateManager.Instance.NetFinished();
        _net.SetActive(false);
        UIManager.Instance.DisableStoppedIcon();
        ResetSpeed();
    }

    /// <summary>
    /// Stops the player and disables this component.
    /// </summary>
    void StopMovement()
    {
        HaltMovement();
        this.enabled = false;
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
        SetDodgeForeignValues(true);
        StartCoroutine(EndDodgeRtn());
    }
    /// <summary>
    /// Sets the values in this class that are needed for dodging.
    /// </summary>
    void SetDodgeLocalValues()
    {
        _canDodgeTime = Time.time + _dodgeCooldown;
        _dodging = true;
        _curSpeed = _dodgeSpeed;
        DecrementStamina();
    }

    /// <summary>
    /// Sets the values outside this class that are part of the dodge mechanic.
    /// </summary>
    void SetDodgeForeignValues(bool dodgeActive)
    {
        if (dodgeActive)
        {
            PlayerStateManager.Instance.DodgeStarted();

            _playerSpriteRend.color = _fadedAlpha;
            _heart.EnableDodgeInvulnerability();
        }
        else
        {
            PlayerStateManager.Instance.DodgeFinished();

            _playerSpriteRend.color = Color.white;
            _heart.DisableDodgeInvulnerability();
        }
    }
    /// <summary>
    /// Ends the dodge after dodge duration.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndDodgeRtn()
    {
        yield return HM.WaitTime(_dodgeDuration);
        _dodging = false;
        ResetSpeed();
        SetDodgeForeignValues(false);
    }

    #endregion

    #region Stamina

    void UpdateStamina()
    {
        if (PlayerIsMoving() && _sprinting)
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

        EndStaminaCooldown();
    }
    void EndStaminaCooldown()
    {
        UIManager.Instance.StaminaCooldownVisual_Off();
        _staminaOnCooldown = false;
    }

    public void CollectStaminaBoostPowerup(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.StaminaBoost)
        {
            RemoveSlowPowerupEffect();

            _staminaGainPerSecond = _boostedStaminaGain;

            if (_disableStaminaBoostRtn != null) StopCoroutine(_disableStaminaBoostRtn);
            _disableStaminaBoostRtn = StartCoroutine(DisableStaminaBoostRtn());

            UIManager.Instance.Enable_StaminaBoostIcon();
        }
    }
    IEnumerator DisableStaminaBoostRtn()
    {
        yield return HM.WaitTime(_staminaBoostPowerupActiveTime);
        RemoveStaminaBoostEffect();
    }
    void RemoveStaminaBoostEffect()
    {
        if (_disableStaminaBoostRtn != null) StopCoroutine(_disableStaminaBoostRtn);

        _staminaGainPerSecond = _defStaminaGain;
        UIManager.Instance.Disable_StaminaBoostIcon();
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
        if (PlayerStateManager.Instance.PlayerInNet)
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
        if (PlayerStateManager.Instance.PlayerInNet)
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

    void SetHeavyAttackSpeed()
    {
        _curSpeed = _heavyAtkMoveSpeed;
    }
    /// <summary>
    /// Sets current speed based on the player's current movement state.
    /// </summary>
    void ResetSpeed()
    {
        if (PlayerStateManager.Instance.PlayerInNet)
            return;
        if (_sprinting)
        {
            _curSpeed = _sprintSpeed;
        }
        else if (_dodging)
        {
            _curSpeed = _dodgeSpeed;
        }
        else
        {
            _curSpeed = _walkSpeed;
        }
    }

    void CollectSlowPowerup(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.Negative_Slow)
        {
            RemoveStaminaBoostEffect();

            _slowedMovementPrct = 0.6f;
            UIManager.Instance.Enable_SlowedIcon();

            if (_removeSlowRtn != null) StopCoroutine(_removeSlowRtn);
            _removeSlowRtn = StartCoroutine(RemoveSlowRtn());
        }
    }
    IEnumerator RemoveSlowRtn()
    {
        yield return HM.WaitTime(_slowDuration);
        RemoveSlowPowerupEffect();
    }
    void RemoveSlowPowerupEffect()
    {
        if (_removeSlowRtn != null) StopCoroutine(_removeSlowRtn);

        _slowedMovementPrct = 1;
        UIManager.Instance.Disable_SlowedIcon();
    }


}