#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Javelins : Weapon 
{
#region Variables

    [SerializeField] GameObject _javelinProjectilePref;

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

    public override void Attack()
    {
        if (CooldownFinished())
        {
            Instantiate(_javelinProjectilePref, transform.position, Quaternion.identity);
            StartCooldown();
            //player throw animatino
            //sound effect
        }
    }


}