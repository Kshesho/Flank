#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles the whip's behavior.
/// </summary>
public class Whip : Weapon 
{
#region Variables

    [SerializeField] Animator _whipAnim;

#endregion

    private void OnEnable()
    {
        Events.OnPlayerHeavyAttackCancelled += CancelAttack;
    }
    private void OnDisable()
    {
        Events.OnPlayerHeavyAttackCancelled -= CancelAttack;
    }

    public override void Attack()
    {
        if (CooldownFinished())
        {
            StartCooldown();
            PlayerStateManager.Instance.HeavyAttackStarted();
            _whipAnim.SetTrigger("whip");
            //sound effect(s)
        }
    }

    /// <summary>
    /// Called when PlayerStateManager.HeavyAttackFinished is called with a cancelled state.
    /// </summary>
    void CancelAttack()
    {
        _whipAnim.SetTrigger("cancelAttack");
    }


}