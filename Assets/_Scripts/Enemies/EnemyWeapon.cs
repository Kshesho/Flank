#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Collides with and damages player and damagables
/// </summary>
public class EnemyWeapon : MonoBehaviour 
{
#region Variables

    [SerializeField] int _damage = 5;

    [SerializeField] CollisionTrigger _collisionTrigger;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            _collisionTrigger.CallOnCollision(_damage);
        }
    }

#endregion


}