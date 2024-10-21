#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles changing player's animation state parameters based on input
/// </summary>
public class PlayerAnimStateChanger : MonoBehaviour 
{
#region Variables

    PlayerStateManager _psm;
    [SerializeField] Animator _anim;
    string _currentState;

    //State Names
    const string IDLE = "Idle_Blend Tree";//interrupted by everything but DODGE
    const string WALK = "Walk_Blend Tree";//interrupted by everything
    const string SPRINT = "Sprint_Blend Tree";//interrupted by everything except IDLE
    const string DODGE = "Dodge_Blend Tree";//will always finish

    const string HIT = "Hit_Blend Tree";//interrupted by DODGE

    const string ATTACK_SWORD = "Attack-Sword_Blend Tree";//interrupted by DODGE
    const string ATTACK_WHIP = "Player_Attack-Whip_N";//interrupted by DODGE, SPRINT
    const string THROW = "Player_Throw_N";

#endregion
#region Base Methods
	
    void Start()
    {
        _psm = PlayerStateManager.Instance;
    }

	void Update () 
    {
        if (GameManager.Instance.GamePaused)
            return;

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        ExitAnimations();

        //Don't set animation direction (lock it), or change state, while player is dodging
        if (_psm.PlayerIsDodging)
        {
            ChangeAnimState(DODGE);
            return;
        }

        //Change the direction the player is facing (animation direction) based on the movement direction (input axes)
        if (hInput != 0 || vInput != 0)
        {
            _anim.SetFloat("Horizontal", hInput);
            _anim.SetFloat("Vertical", vInput);
        }

        UpdateAnimState();
	}

#endregion

//======================================================================================
    /// <summary>
    /// Changes the current animator state to <paramref name="newState"/> if that isn't already the current state.
    /// </summary>
    /// <param name="newState"></param>
    void ChangeAnimState(string newState)
    {
        if (newState == _currentState)
            return;

        _currentState = newState;
        _anim.Play(newState);
    }
//======================================================================================
    
    /// <summary>
    /// Changes the Animator state based on the actions the player is currently performing.
    /// </summary>
    void UpdateAnimState()
    {
        if (_psm.PlayerIsBeingHit)
        {
            ChangeAnimState(HIT);
        }
        //=====Attacking
        else if (_psm.PlayerIsAttacking)
        {
            ChangeAnimState(ATTACK_SWORD);
        }
        else if (_psm.PlayerIsAttacking_Heavy)
        {
            ChangeAnimState(ATTACK_WHIP);
        }
        else if (_psm.PlayerIsThrowing)
        {
            ChangeAnimState(THROW);
        }
        //=====Moving
        else if (_psm.PlayerIsMoving)
        {
            if (_psm.PlayerIsSprinting)
                ChangeAnimState(SPRINT);
            else            
                ChangeAnimState(WALK);
        }
        else        
            ChangeAnimState(IDLE);
    }

    /// <summary>
    /// If a non-looping animation is playing, ends that animation after it fully plays (by setting PlayerStateManager values).
    /// </summary>
    void ExitAnimations()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        //normalized time returns a fraction of the total duration of the animation. 1 = finished. 2 = looped once.
        if (stateInfo.normalizedTime >= 1 && stateInfo.normalizedTime < 2)
        {
            switch (_currentState)
            {
                case DODGE:
                    _psm.DodgeFinished();
                    break;
                case HIT:
                    _psm.HitFinished();
                    break;
                case ATTACK_SWORD:
                    _psm.AttackFinished();
                    break;
                case ATTACK_WHIP:
                    _psm.HeavyAttackFinished(true);
                    break;
                case THROW:
                    _psm.ThrowFinished();
                    break;
            }
        }
    }

    // Promblem: 
    // Solution: 


}