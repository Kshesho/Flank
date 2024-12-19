#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Holds and changes player state variables.
/// </summary>
public class PlayerStateManager : MonoSingleton<PlayerStateManager> 
{
#region Variables

    public bool PlayerIsMoving {  get; private set; }
    public bool PlayerIsSprinting { get; private set; }
    public bool PlayerInNet { get; private set; }
    public bool PlayerIsDodging { get; private set; }
    public bool PlayerIsAttacking { get; private set; }
    public bool PlayerIsAttacking_Heavy { get; private set; }
    public bool PlayerIsThrowing { get; private set; }
    public bool PlayerIsBeingHit { get; private set; }
    public bool PlayerIsDead { get; private set; }
    /// <summary>
    /// Returns true if player is currently attacking, dodging, or being hit.
    /// </summary>
    /// <returns></returns>
    public bool PlayerCanPrimaryAttack()
    {
        //Don't need to check if attacking here because of attack cooldown time
        if (PlayerIsDodging || PlayerIsThrowing)
            return false;

        return true;
    }
    public bool PlayerCanSecondaryAttack()
    {// TODO: if attacking, cancel attack to throw. If dodging, throw without animation
        //Don't need to check if throwing here becuase of throw cooldown
        if (PlayerIsDodging || PlayerIsAttacking || PlayerIsAttacking_Heavy)
            return false;
        
        return true;
    }

#endregion

    void CancelAllStates_ButDeath()
    {
        PlayerIsMoving = false;
        PlayerIsSprinting = false;
        PlayerInNet = false;
        PlayerIsDodging = false;
        PlayerIsAttacking = false;
        PlayerIsAttacking_Heavy = false;
        PlayerIsThrowing = false;
        PlayerIsBeingHit = false;
    }

    public void MovementStarted()
    {
        PlayerIsMoving = true;
    }
    public void MovementStopped()
    {
        PlayerIsMoving = false;
    }

    public void SprintStarted()
    {
        PlayerIsSprinting = true;

        //Sprint interrupts heavy attack
        if (PlayerIsAttacking_Heavy) HeavyAttackFinished(false);
    }
    public void SprintStopped()
    {
        PlayerIsSprinting = false;
    }

    public void DodgeStarted()
    {
        PlayerIsDodging = true;

        //Dodge interrupts all attacks
        if (PlayerIsAttacking) AttackFinished();
        if (PlayerIsAttacking_Heavy) HeavyAttackFinished(false);
        if (PlayerIsThrowing) ThrowFinished();
    }
    public void DodgeFinished()
    {
        PlayerIsDodging = false;
    }


    public void AttackStarted()
    {
        PlayerIsAttacking = true;
    }
    public void AttackFinished()
    {
        PlayerIsAttacking = false;
    }
    public void HeavyAttackStarted()
    {
        PlayerIsAttacking_Heavy = true;
        Events.OnPlayerHeavyAttackStarted?.Invoke();
    }
    public void HeavyAttackFinished(bool attackCompleted)
    {
        PlayerIsAttacking_Heavy = false;

        if (attackCompleted)
        {
            Events.OnPlayerHeavyAttackFinished?.Invoke();
        }
        else Events.OnPlayerHeavyAttackCancelled?.Invoke();
    }

    public void ThrowStarted()
    {
        PlayerIsThrowing = true;
    }
    public void ThrowFinished()
    {
        PlayerIsThrowing = false;
    }
 

    public void HitStarted()
    {
        PlayerIsBeingHit = true;
        Events.OnPlayerDamaged?.Invoke();
    }
    public void HitFinished()
    {
        PlayerIsBeingHit = false;
    }

    public void NetStarted()
    {
        PlayerInNet = true;
    }
    public void NetFinished()
    {
        PlayerInNet = false;
    }


    public void PlayerDied()
    {
        CancelAllStates_ButDeath();

        PlayerIsDead = true;
        Events.OnPlayerDeath?.Invoke();
    }


}