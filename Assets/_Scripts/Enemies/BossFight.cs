#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// State Machine and animation controller for boss fight.
/// </summary>
public class BossFight : MonoBehaviour 
{
#region Variables

    public BossState CurState { get; private set; } = BossState.Idle;

    [SerializeField] Animator _myAnim; //animates transform properties
    [SerializeField] Animator _spriteAnim; //animates sprite properties

    //Sprite Animation States
    const string IDLE = "Boss_Idle_S";
    const string WALK = "Boss_Walk_S";
    const string RAISE_AXE = "Boss_RaiseAxe_S";
    const string BLOCK = "Block_BlendTree";

#endregion
#region Base Methods

    void Start () 
    {
        ChangeState(BossState.Spawning);
	}
	
	void Update () 
    {
		if (CurState == BossState.Blocking ||
            CurState == BossState.Atk_Chargeup)
        {
            FacePlayer();
        }
	}

#endregion

    /// <summary>
    /// This will (mostly?) be called from external state-specific objects
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(BossState newState)
    {
        if (CurState != newState)
        {
            CurState = newState;
            ActivateNewState();
        }
    }

    void ActivateNewState()
    {
        switch (CurState)
        {
            case BossState.Idle:
                break;

            case BossState.Spawning:
                //animate the sprite position from the top of the screen to the center (w/ special animation at the end to finish it off)
                _spriteAnim.Play(WALK);
                break;
            case BossState.Blocking:
                //set sprite animation to block
                _spriteAnim.Play(IDLE);
                break;
            case BossState.Atk_Chargeup:
                //set sprite animation to chargeup
                break;
            case BossState.Attacking:
                break;
            case BossState.Atk_Cooldown:
                break;
            case BossState.Summoning:
                break;
            case BossState.Dying:
                break;
            default:
                break;
        }
    }

    void FacePlayer()
    {
        Vector2 dirToPlayer = GameManager.Instance.PlayerPosition() - (Vector2)transform.position;
        dirToPlayer.Normalize();

        _spriteAnim.SetFloat("Horizontal", dirToPlayer.x);
        _spriteAnim.SetFloat("Vertical", dirToPlayer.y);
    }

    /// <summary>
    /// Called via Animator keyframe in Entrance animation.
    /// </summary>
    public void PlayRaiseAxe()
    {
        _spriteAnim.Play(RAISE_AXE);
    }
    /// <summary>
    /// Called via Animator keyframe in Entrance animation.
    /// </summary>
    public void FinishSpawning()
    {
        ChangeState(BossState.Blocking);
    }

}

///
/// BOSS STATES
//SPAWNING
//- Walk from the top of the scrren to the middle

//BLOCKING
//- take no damage from player attacks
//- face player

//CHARGING UP
//- take damage
//- face player

//ATTACKING
//- take damage
//- damage player

//COOL DOWN
//- take damage

//SUMMONING
//- take damage
//- spawn zombies