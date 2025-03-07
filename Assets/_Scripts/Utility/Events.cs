#region Using Statements
using System;
using UnityEngine;
#endregion

/// <summary>
/// Holds all events that aren't specific enough to be contained in a separate class.
/// </summary>
public class Events 
{
    /// <summary>
    /// Event for when collisions happen and damage is dealt.
    /// </summary>
    public static Action<Collider2D, int> OnCollide;

    /// <summary>
    /// Event for when player melee attack collides with boss.
    /// </summary>
    public static Action<int> OnBossCollide_Melee;
    /// <summary>
    /// Event for when player projectile collides with boss.
    /// </summary>
    public static Action<int, GameObject> OnBossCollide_Projectile;
    public static Action OnBossDeath;

    /// <summary>
    /// Event for when the Player collects a Powerup.
    /// </summary>
    public static Action<PowerupType> OnPowerupCollected;
    /// <summary>
    /// Event for when a powerup enters the player's Powerup Magnet.
    /// </summary>
    public static Action<GameObject> OnPowerupMagnetized;
    /// <summary>
    /// Event for when a powerup stops being affected by the player's Powerup Magnet.
    /// </summary>
    public static Action<GameObject> OnPowerupUnmagnetized;

    /// <summary>
    /// Event for when the player is hit by the Huntress' net.
    /// </summary>
    public static Action OnPlayerNetted;

    /// <summary>
    /// Event for when the player gets damaged.
    /// </summary>
    public static Action OnPlayerDamaged;
    /// <summary>
    /// Event for when the player dies.
    /// </summary>
    public static Action OnPlayerDeath;

    /// <summary>
    /// Event for when the player starts a heavy attack.
    /// </summary>
    public static Action OnPlayerHeavyAttackStarted;
    /// <summary>
    /// Event for when the player's heavy attack is cancelled.
    /// </summary>
    public static Action OnPlayerHeavyAttackCancelled;
    /// <summary>
    /// Event for when the player's heavy attack fully finishes.
    /// </summary>
    public static Action OnPlayerHeavyAttackFinished;

    /// <summary>
    /// Event for when the player's Boomerang weapon projectile returns to them.
    /// Stores whether or not the boomerang hit a target.
    /// </summary>
    public static Action<bool> OnBoomerangReturned;

}