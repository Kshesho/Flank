#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narzioth.Utilities;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class VigilanteKnife : EnemyProjectile 
{
#region Variables

    bool _targetingPlayer = true;

#endregion
#region Base Methods

    void Start () 
    {
        transform.LookAt2D(GameManager.Instance.PlayerTransform());
	}
	
#endregion

    //Only collide with current target
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (_targetingPlayer)
        {
            if (other.CompareTag(Tags.Player))
            {
                Events.OnCollide?.Invoke(other, _damage);
                Destroy(this.gameObject);
            }
        }
        else //target powerup
        {
            if (other.CompareTag(Tags.Powerup))
            {
                Events.OnCollide?.Invoke(other, 1);
                Destroy(this.gameObject);
            }
        }
    }

    public void TargetPowerup(Transform powerupTrans)
    {
        transform.LookAt2D(powerupTrans);
        _targetingPlayer = false;
    }

}