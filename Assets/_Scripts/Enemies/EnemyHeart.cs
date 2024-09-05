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
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] Collider2D _thisCollider;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
    }

    private void OnDisable()
    {
        Events.OnCollide -= TakeDamage;
    }

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

#endregion

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider)
            return;

        _health -= damage;
        if (_health < 1)
        {
            Destroy(_enemyContainer);
        }
    }


}