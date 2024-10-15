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

    public bool PlayerIsAttacking { get; private set; }
    public bool PlayerIsDodging { get; private set; }
    public bool PlayerIsBeingHit { get; private set; }
    /// <summary>
    /// Returns true if player is currently attacking, dodging, or being hit.
    /// </summary>
    /// <returns></returns>
    public bool PlayerIsBusy()
    {
        if (PlayerIsAttacking || PlayerIsDodging || PlayerIsBeingHit)
            return true;

        return false;
    }

#endregion

    void ResetStateVariables()
    {
        PlayerIsAttacking = false;
        PlayerIsDodging = false;
        PlayerIsBeingHit = false;
    }

    public void AttackStarted()
    {
        PlayerIsAttacking = true;
    }
    public void AttackFinished()
    {
        PlayerIsAttacking = false;
    }

    public void DodgeStarted()
    {
        PlayerIsDodging = true;
    }
    public void DodgeFinished()
    {
        PlayerIsDodging = false;
    }

    public void HitStarted()
    {
        PlayerIsBeingHit = true;
    }
    public void HitFinished()
    {
        PlayerIsBeingHit = false;
    }


}