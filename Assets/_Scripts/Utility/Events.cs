#region Using Statements
using System;
#endregion

/// <summary>
/// Holds all events that aren't specific enough to be contained in a separate class.
/// </summary>
public class Events 
{
    /// <summary>
    /// Event for when collisions happen and damage is dealt.
    /// </summary>
    public static Action<int> OnCollide;


}