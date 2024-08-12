#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles behavior for projectiles
/// </summary>
public class Projectile : MonoBehaviour 
{
#region Variables

    [SerializeField] float _moveSpeed = 5f;

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
        transform.Translate(Vector2.up * _moveSpeed * Time.deltaTime, Space.Self);
	}

#endregion

    ///When I collide with something
    /// how do I know what I collided with?
    /// where do I handle collision result?
}