#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles input and activating player's weapons when appropriate.
/// </summary>
public class PlayerWeaponController : MonoBehaviour 
{
#region Variables

    [SerializeField] GameObject _ninjaStarPref; 

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(_ninjaStarPref, transform.position, Quaternion.identity);
        }
	}

#endregion



}