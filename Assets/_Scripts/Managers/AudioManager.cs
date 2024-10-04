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

	[SerializeField] AudioSource _javelinPickupAuSrc, _shieldPickupAuSrc, _dodgeCooldownAuSrc;

	[Header("Enemy SFX")]
	[SerializeField] AudioSource _enemyDeathAuSrc;
	[SerializeField] AudioClip[] _enemyDeathAuClips;

#endregion
#region Base Methods

    void OnEnable()
	{
		Events.OnPowerupCollected += PlayPowerupPickup;
	}
	void OnDisable()
	{
		Events.OnPowerupCollected -= PlayPowerupPickup;
	}

#endregion

	public void PlayMainTheme()
	{
		_musicAuSrc.Play();
	}

	public void PlaySwordSwing()
	{
		int rand = Random.Range(0, _swordSwingAuClips.Length);
		_swordSwingAuSrc.clip = _swordSwingAuClips[rand];
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
			case PowerupType.DodgeCooldown:
				_dodgeCooldownAuSrc.Play();
				break;
			default:
				Debug.LogError($"No audio functionality assigned for [{powerupType}] powerup type!");
				break;
		}
	}

	public void PlayEnemyDeath()
	{
		_enemyDeathAuSrc.pitch = Random.Range(0.9f, 1.1f);
		int rand = Random.Range(0, _enemyDeathAuClips.Length);
		_enemyDeathAuSrc.PlayOneShot(_enemyDeathAuClips[rand]);
	}


}