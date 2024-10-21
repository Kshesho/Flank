#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Sword : Weapon 
{
#region Variables

    [SerializeField] PlayerAnimStateChanger _playerAnimChanger;
    [SerializeField] Animator _swordAnim;

#endregion

    public override void Attack()
    {
        if (CooldownFinished())
        {
            StartCooldown();
            _swordAnim.SetTrigger("swing");
            PlayerStateManager.Instance.AttackStarted();
            AudioManager.Instance.PlaySwordSwing();
        }
    }


}