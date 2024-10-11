#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Defines the Shurikens thrown weapon behavior.
/// </summary>
public class Shurikens : Weapon 
{

#region Variables

    [SerializeField] GameObject _shurikenProjectilePref;
    Vector3 _spawnOffset = new Vector3(0, 0.72f, 0);

#endregion

    public override void Attack()
    {
        if (CooldownFinished())
        {
            //check for and deduct ammo
            Instantiate(_shurikenProjectilePref, transform.position + _spawnOffset, Quaternion.identity);
            StartCooldown();
            //player throw animation
            //sound effect
        }
    }


}