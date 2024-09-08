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

    int _maxHealth = 10;
    int _currentHealth;
    bool _deathless;
    public int CurrentHealth { get { return _currentHealth; } }
    [SerializeField] GameObject _playerContainer;
    [SerializeField] Collider2D _thisCollider;

#endregion
#region Base Methods

    void Awake()
    {
        InitHealth();
    }
    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
    }
    void OnDisable()
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

    void InitHealth()
    {
        _currentHealth = _maxHealth;
    }

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider ||
            _deathless)
            return;

        _currentHealth -= damage;
        Debug.Log("Health: " +  _currentHealth);
        // TODO: update UI
        if (_currentHealth < 1)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(_playerContainer);
        Events.OnPlayerDeath?.Invoke();
    }

    public void EnableDeathless()
    {
        _deathless = true;
    }
    public void DisableDeathless()
    {
        _deathless = false;
    }


}