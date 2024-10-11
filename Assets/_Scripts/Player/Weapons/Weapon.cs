#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// The base class for all player weapons.
/// </summary>
public abstract class Weapon : MonoBehaviour 
{
#region Variables

    [SerializeField] protected float _cooldownTime;
    protected float _canAtkTime;

#endregion
#region Base Methods

    void Awake()
    {
        
    }

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

#endregion

    public abstract void Attack();

    /// <summary>
    /// Returns true if _canAtkTime is less than Time.time.
    /// </summary>
    /// <returns></returns>
    protected bool CooldownFinished()
    {
        if (_canAtkTime < Time.time)
            return true;
        return false;
    }

    /// <summary>
    /// Adds the current time + cooldown to _canAtkTime.
    /// </summary>
    protected void StartCooldown()
    {
        _canAtkTime = Time.time + _cooldownTime;
    }


}