#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles triggering camera shake animations.
/// </summary>
public class CameraShake : MonoBehaviour 
{
#region Variables

    [SerializeField] Animator _anim;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnPlayerDamaged += CameraShake_Small;
        Events.OnPlayerDeath += CameraShake_Large;
    }

    void OnDisable() 
    {
		Events.OnPlayerDamaged -= CameraShake_Small;
        Events.OnPlayerDeath -= CameraShake_Large;
	}

#endregion

    void CameraShake_Small()
    {
        _anim.SetTrigger("smallShake");
    }

    void CameraShake_Large()
    {
        _anim.SetTrigger("largeShake");
    }

}