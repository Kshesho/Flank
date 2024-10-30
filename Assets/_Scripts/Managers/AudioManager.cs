#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narzioth.Utilities;
#endregion

/// <summary>
/// Handles playing audio sources.
/// </summary>
public class AudioManager : MonoSingleton<AudioManager>
{
#region Variables

	[SerializeField] AudioSource _musicAuSrc;

	[Header("Player SFX")]
	[SerializeField] AudioSource _swordSwingAuSrc;
	[SerializeField] AudioClip[] _swordSwingAuClips;
	[SerializeField] AudioSource _playerDamagedAuSrc;
	[SerializeField] AudioClip[] _playerDamagedAuClips;
	[SerializeField] AudioClip _playerDeathClip;

	[SerializeField] AudioSource _javelinPickupAuSrc, _shieldPickupAuSrc, _staminaBoostAuSrc;
	[SerializeField] AudioSource _ammoPickupAuSrc;

	[Header("Enemy SFX")]
	[SerializeField] AudioSource _enemyDeathAuSrc;
	[SerializeField] AudioClip[] _enemyDeathAuClips;
	[SerializeField] AudioSource _enemyImpactAuSrc;

#endregion
#region Base Methods

    void OnEnable()
	{
		Events.OnPowerupCollected += PlayPowerupPickup;
		Events.OnPlayerDeath += PlayPlayerDeath;
	}
	void OnDisable()
	{
		Events.OnPowerupCollected -= PlayPowerupPickup;
		Events.OnPlayerDeath -= PlayPlayerDeath;
	}

#endregion

	public void PlayMainTheme()
	{
		_musicAuSrc.Play();
	}

	public void PlayPlayerHurt()
	{
		_playerDamagedAuSrc.clip = RandomClip(_playerDamagedAuClips);
		_playerDamagedAuSrc.Play();
	}
	void PlayPlayerDeath()
	{
		_playerDamagedAuSrc.clip = _playerDeathClip;
		_playerDamagedAuSrc.Play();
	}

	public void PlaySwordSwing()
	{
		_swordSwingAuSrc.clip = RandomClip(_swordSwingAuClips);
		_swordSwingAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_swordSwingAuSrc.Play();
	}

	void PlayPowerupPickup(PowerupType powerupType)
	{
		switch (powerupType)
		{
			case PowerupType.Javelin:
				_javelinPickupAuSrc.Play();
				break;
			case PowerupType.Shield:
				_shieldPickupAuSrc.Play();
				break;
			case PowerupType.StaminaBoost:
				_staminaBoostAuSrc.Play();
				break;
			case PowerupType.Ammo:
				PlayAmmoPickup();
				break;
			default:
				Debug.LogError($"No audio functionality assigned for [{powerupType}] powerup type!");
				break;
		}
	}
	void PlayAmmoPickup()
	{
		_ammoPickupAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_ammoPickupAuSrc.Play();
	}

	public void PlayEnemyDeath()
	{
		_enemyDeathAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_enemyDeathAuSrc.PlayOneShot(RandomClip(_enemyDeathAuClips));
	}

	public void PlayEnemyImpact()
	{
		_enemyImpactAuSrc.Play();
	}

    #region Return Types

	/// <summary>
	/// 
	/// </summary>
	/// <param name="_clips"></param>
	/// <returns>A random clip from <paramref name="_clips"/></returns>
	AudioClip RandomClip(AudioClip[] _clips)
	{
		int rand = Random.Range(0, _clips.Length);
		return _clips[rand];
	}

    #endregion


}