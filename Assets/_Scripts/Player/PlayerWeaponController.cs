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
    float _canThrowTime = -1;
    bool NinjaStarCooldownFinished()
    {
        if (_canThrowTime < Time.time)
            return true;
        return false;
    }
    Vector3 _spawnOffset = new Vector3(0, 0.72f, 0);

    float _swordCooldownTime = 2f;
    [SerializeField] Animator _swordAnim;
    float _canSwingTime = -1;
    bool SwordCooldownFinished()
    {
        if (_canSwingTime < Time.time)
            return true;
        return false;
    }

#endregion
#region Base Methods

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (NinjaStarCooldownFinished())
            {
                Instantiate(_ninjaStarPref, transform.position + _spawnOffset, Quaternion.identity);
                _canThrowTime = Time.time + _cooldownTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (SwordCooldownFinished())
            {
                _canSwingTime = Time.time + _swordCooldownTime;
                _swordAnim.SetTrigger("swing");
            }
        }
	}

#endregion



}