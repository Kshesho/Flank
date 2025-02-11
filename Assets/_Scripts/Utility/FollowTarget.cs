#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Keeps position at target location.
/// </summary>
public class FollowTarget : MonoBehaviour 
{
#region Variables

    [SerializeField] Transform _target;
    [SerializeField] float _xOffset, _yOffset;

#endregion
#region Base Methods
	
	void Update () 
    {
        if (_target != null)
        {
            transform.position = new Vector3(
                _target.position.x + _xOffset,
                _target.position.y + _yOffset, 
                this.transform.position.z);
        }
	}

#endregion



}