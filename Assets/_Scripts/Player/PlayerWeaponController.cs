#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#endregion

/// <summary>
/// Handles input and activating player's weapons when appropriate.
/// </summary>
public class PlayerWeaponController : MonoBehaviour 
{
#region Variables

    [SerializeField] GameObject _ninjaStarPref;
    [SerializeField] float _cooldownTime = 0.25f;
    float _canFireTime = -1;
    Vector3 _spawnOffset = new Vector3(0, 0.72f, 0);

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_canFireTime < Time.time)//cooldown finished
            {
                Instantiate(_ninjaStarPref, transform.position + _spawnOffset, Quaternion.identity);
                _canFireTime = Time.time + _cooldownTime;
            }
        }
	}

#endregion



}