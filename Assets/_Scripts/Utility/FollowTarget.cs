#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class FollowTarget : MonoBehaviour 
{
#region Variables

    [SerializeField] Transform _target;

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
        if (_target != null)
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, this.transform.position.z);
        }
	}

#endregion



}