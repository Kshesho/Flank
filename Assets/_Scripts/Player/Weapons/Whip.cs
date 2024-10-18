#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Whip : Weapon 
{
#region Variables

    [SerializeField] Animator _whipAnim;
    [SerializeField] PlayerAnimStateChanger _playerAnimStateChanger;

#endregion

    public override void Attack()
    {
        if (CooldownFinished())
        {
            StartCooldown();
            _whipAnim.SetTrigger("whip");
            _playerAnimStateChanger.AttackGeneric();
            //sound effect(s)
        }
    }


}