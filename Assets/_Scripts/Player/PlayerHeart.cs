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
    [SerializeField] PlayerShieldsVisual _shieldsVisual;
    [SerializeField] SpriteRenderer _playerSpriteRend;

    Color _fadedAlpha = new Color(1f, 1f, 1f, 0.3f);

    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
    bool _dodgeInvulnerability;
    public int CurrentHealth { get { return _currentHealth; } }
    [SerializeField] GameObject _playerContainer;
    [SerializeField] Collider2D _thisCollider;

    bool _shieldsActive;
    int _shieldCount_DONTALTER;
    int ShieldCount 
    { 
        get
        {
            return _shieldCount_DONTALTER;
        }
        set 
        {
            if (value < 0)
                _shieldCount_DONTALTER = 0;

            else if (value > 3)
                _shieldCount_DONTALTER = 3;

            else _shieldCount_DONTALTER = value;
        } 
    }

    bool _damageCooldownActive;
    float _damageCooldown = 0.8f;//This must match the time of the "Damaged_Flash" animation

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
        Events.OnPowerupCollected += AddAShield;
        Events.OnPowerupCollected += Heal;
    }
    void OnDisable()
    {
        Events.OnCollide -= TakeDamage;   
        Events.OnPowerupCollected -= AddAShield;
        Events.OnPowerupCollected -= Heal;
    }

    void Start () 
    {
        InitHealth();
	}


#endregion

    void InitHealth()
    {
        _currentHealth = _maxHealth;
        UIManager.Instance.UpdateHealthUI(0, _maxHealth, _maxHealth);
    }

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider ||
            _dodgeInvulnerability || _damageCooldownActive)
            return;

        if (_shieldsActive)
        {
            RemoveAShield();
            StartCoroutine(DamageCooldownRtn());
            return;
        }

        int previousHealth = _currentHealth;
        _currentHealth -= damage;
        _animStateChanger.Hit();
        UIManager.Instance.UpdateHealthUI(previousHealth, _currentHealth, _maxHealth);
        StartCoroutine(DamageCooldownRtn());
        if (_currentHealth < 1)
        {
            Death();
        }
    }

    IEnumerator DamageCooldownRtn()
    {
        _damageCooldownActive = true;
        _playerSpriteRend.color = _fadedAlpha;
        yield return HM.WaitTime(_damageCooldown);
        _playerSpriteRend.color = Color.white;
        _damageCooldownActive = false;
    }

    void Heal(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.HealthPotion)
        {
            int previousHealth = _currentHealth;
            _currentHealth += 50;
            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
            
            UIManager.Instance.UpdateHealthUI(previousHealth, _currentHealth, _maxHealth);
        }
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

    void AddAShield(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.Shield)
        {
            _shieldsVisual.AddAShield();
            ShieldCount++;
            _shieldsActive = true;
        }
    }
    void RemoveAShield()
    {
        _shieldsVisual.RemoveAShield();
        ShieldCount--;
        if (ShieldCount == 0)
            _shieldsActive = false;
    }


}