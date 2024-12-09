#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class KnightWeapon : MonoBehaviour 
{
#region Variables

    [SerializeField] KnightAnimStateChanger _animStateChanger;
    [SerializeField] GameObject _projectileGroupPref;

#endregion

    public void Attack()
    {
        _animStateChanger.PlayAttack();
    }
    

    //called via keyframe in 'Knight_charge_color blink' animation
    void Shoot()
    {
        GameObject newProj = Instantiate(_projectileGroupPref, this.transform.position, Quaternion.identity);
        newProj.transform.LookAt2D(GameManager.Instance.PlayerTransform());
    }



}