#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles logic for the knight's weapon attacks.
/// </summary>
public class KnightWeapon : MonoBehaviour 
{
#region Variables

    [SerializeField] KnightAnimStateChanger _animStateChanger;
    [SerializeField] KnightMovement _movement;
    [SerializeField] GameObject _projectileGroupPref;
    
    [SerializeField] float _range = 5;
    [SerializeField] float _meleeCooldown = 4;
    float _meleeCooldownTimer;

#endregion
#region Base Methods

    void Update()
    {
        if (PlayerInRange())
        {
            if (_meleeCooldownTimer < Time.time)
            {
                SwordAttack();
            }
        }
    }

#if UNITY_EDITOR
    //Visualize the range of the proximity attack in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
#endif

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

    bool PlayerInRange()
    {
        float distance = Vector2.Distance(this.transform.position, GameManager.Instance.PlayerTransform().position);
        if (distance <= _range)
            return true;
        
        return false;
    }

    void SwordAttack()
    {
        //will this attack always interrupt the ranged attack chargeup?
        _meleeCooldownTimer = Time.time + _meleeCooldown;
        _movement.StopMoving();
        _animStateChanger.SwordAttack();
    }

}