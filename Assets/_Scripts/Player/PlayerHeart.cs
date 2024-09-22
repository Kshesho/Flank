#region Using Statements
using Narzioth.Utilities;
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

    [SerializeField] PlayerAnimStateChanger _animStateChanger;

    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
    bool _dodgeInvulnerability;
    public int CurrentHealth { get { return _currentHealth; } }
    [SerializeField] GameObject _playerContainer;
    [SerializeField] Collider2D _thisCollider;

    [SerializeField] GameObject _shield;
    bool _shieldActive;

    bool _damageCooldownActive;
    float _damageCooldown = 0.8f;//This must match the time of the "Damaged_Flash" animation

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
        Events.OnPowerupCollected += ActivateShield;
    }
    void OnDisable()
    {
        Events.OnCollide -= TakeDamage;   
        Events.OnPowerupCollected -= ActivateShield;
    }

    void Start () 
    {
        InitHealth();
	}
	
	void Update () 
    {
		
	}


#endregion

    void InitHealth()
    {
        _currentHealth = _maxHealth;
        UIManager.Instance.UpdateHealthUI(_maxHealth, _maxHealth);
    }

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider ||
            _dodgeInvulnerability || _damageCooldownActive)
            return;

        if (_shieldActive)
        {
            DisableShield();
            StartCoroutine(DamageCooldownRtn());
            return;
        }

        _currentHealth -= damage;
        _animStateChanger.Hit();
        UIManager.Instance.UpdateHealthUI(_currentHealth, _maxHealth);
        StartCoroutine(DamageCooldownRtn());
        // TODO: update UI
        if (_currentHealth < 1)
        {
            Death();
        }
    }
    void DisableShield()
    {
        _shieldActive = false;
        _shield.SetActive(false);
    }

    IEnumerator DamageCooldownRtn()
    {
        _damageCooldownActive = true;
        _animStateChanger.PlayDamageFlash();
        yield return HM.WaitTime(_damageCooldown);
        _damageCooldownActive = false;
    }

    void Death()
    {
        Events.OnPlayerDeath?.Invoke();
        Destroy(_playerContainer);
    }

    public void EnableDodgeInvulnerability()
    {
        _dodgeInvulnerability = true;
    }
    public void DisableDodgeInvulnerability()
    {
        _dodgeInvulnerability = false;
    }

    void ActivateShield(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.Shield)
        {
            _shieldActive = true;
            if (!_shield.activeSelf)
                _shield.SetActive(true);
        }
    }


}