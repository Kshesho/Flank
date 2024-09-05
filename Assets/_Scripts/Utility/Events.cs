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
    /// Event for when the Player collects a Powerup.
    /// </summary>
    public static Action<PowerupType> OnPowerupCollected;

    /// <summary>
    /// Event for when the player dies.
    /// </summary>
    public static Action OnPlayerDeath;

}