#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Invokes the Events.OnCollide event after a frame delay
/// </summary>
public class CollisionTrigger : MonoBehaviour 
{
#region Variables

    WaitForEndOfFrame waitAFrame = new WaitForEndOfFrame();

#endregion

    /// <summary>
    /// Calls the OnCollide event
    /// </summary>
    /// <param name="damage"></param>
    public void CallOnCollision(int damage)
    {
        StartCoroutine(WaitAFrame_ThenCallOnCollision(damage));
    }
    /// <summary>
    /// Waits 1 frame before calling the OnCollide event so that subscribing methods have time to 
    /// subscribe before the event is invoked.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAFrame_ThenCallOnCollision(int damage)
    {
        yield return waitAFrame;
        Events.OnCollide?.Invoke(damage);
    }


}