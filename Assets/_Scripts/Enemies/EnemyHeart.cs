#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narzioth.Utilities;
#endregion

/// <summary>
/// Holds Enemy health and damage functionality.
/// </summary>
public class EnemyHeart : MonoBehaviour 
{
#region Variables

    [SerializeField] int _health = 10;

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
        if (other.tag == Tags.PWeapon)
        {
            Events.OnCollide += TakeDamage;
        }
    }

#endregion

    void TakeDamage(int damage)
    {
        ///unsubscribe right after the method is called to avoid it calling it twice
        Events.OnCollide -= TakeDamage;

        _health -= damage;
        Debug.Log("Enemy health: " + _health);
        if (_health < 1)
        {
            Destroy(this.gameObject);
        }
    }


}