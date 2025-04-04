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

    [SerializeField] PlayerShieldsVisual _shieldsVisual;
    [SerializeField] SpriteRenderer _playerSpriteRend;

    Color _fadedAlpha = new Color(1f, 1f, 1f, 0.3f);

    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
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
        Events.OnPowerupCollected += CollectDamagePowerup;
    }
    void OnDisable()
    {
        Events.OnCollide -= TakeDamage;   
        Events.OnPowerupCollected -= AddAShield;
        Events.OnPowerupCollected -= Heal;
        Events.OnPowerupCollected -= CollectDamagePowerup;
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
        if (colliderBeingHit != _thisCollider || _damageCooldownActive)
            return;

        if (_shieldsActive)
        {
            RemoveAShield();
            StartCoroutine(DamageCooldownRtn());
            return;
        }

        int previousHealth = _currentHealth;
        _currentHealth -= damage;
        UIManager.Instance.UpdateHealthUI(previousHealth, _currentHealth, _maxHealth);
        
        if (_currentHealth < 1)
        {
            Death();
        }
        else
        {
            PlayerStateManager.Instance.HitStarted();
            StartCoroutine(DamageCooldownRtn());
            AudioManager.Instance.PlayPlayerHurt();
        }
    }

    void CollectDamagePowerup(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.Negative_Damage)
        {
            TakeDamage(_thisCollider, 20);
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
        PlayerStateManager.Instance.PlayerDied();
        this.gameObject.SetActive(false);
    }

    public void EnableDodgeInvulnerability()
    {
        _thisCollider.enabled = false;
    }
    public void DisableDodgeInvulnerability()
    {
        _thisCollider.enabled = true;
    }

    void AddAShield(PowerupType powerupCollected)
    {
        if (powerupCollected == PowerupType.Shield)
        {
            _shieldsVisual.AddAShield();
            ShieldCount++;
            _shieldsActive = true;
            UIManager.Instance.EnableShieldIcon();
        }
    }
    void RemoveAShield()
    {
        _shieldsVisual.RemoveAShield();
        ShieldCount--;
        if (ShieldCount == 0)
        {
            _shieldsActive = false;
            AudioManager.Instance.Play_PlayerLoseShields();
            UIManager.Instance.DisableShieldIcon();
        }
    }


}