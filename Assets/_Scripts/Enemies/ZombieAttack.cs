#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class ZombieAttack : MonoBehaviour 
{
#region Variables

    [SerializeField] Zombie _zombie;
    [SerializeField] ZombieAnimStateChanger _animStateChanger;
    float _cooldownTimer;
    [SerializeField] float _atkCooldown = 1.1f;

#endregion
#region Base Methods
	
	void Update () 
    {
		if (_zombie.State == ZombieState.Attacking)
        {
            Attack();
        }
	}

#endregion

    void Attack()
    {      
        if (CooldownFinished())
        {
            _cooldownTimer = Time.time + _atkCooldown;
            _animStateChanger.Attack();
        }
        else
        {
            _animStateChanger.Start_WaitingToAttack();
        }
    }
    bool CooldownFinished()
    {
        if (_cooldownTimer < Time.time)
            return true;
        return false;
    }

}