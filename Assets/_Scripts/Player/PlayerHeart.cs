#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Holds player health and damage functionality.
/// </summary>
public class PlayerHeart : MonoBehaviour 
{
#region Variables

    int _maxHealth = 100;
    int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

#endregion
#region Base Methods

    void Awake()
    {
        InitHealth();
    }

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Enemy))
        {
            Events.OnCollide += TakeDamage;
        }
    }

#endregion

    void InitHealth()
    {
        _currentHealth = _maxHealth;
    }

    void TakeDamage(int damage)
    {
        Events.OnCollide -= TakeDamage;

        _currentHealth -= damage;
        Debug.Log("Health: " +  _currentHealth);
        // TODO: update UI
        if (_currentHealth < 1)
        {
            // TODO: death
        }
    }


}