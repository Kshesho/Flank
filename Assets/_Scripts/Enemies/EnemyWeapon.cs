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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Events.OnCollide?.Invoke(other, _damage);
        }
    }

#endregion


}