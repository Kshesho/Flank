#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narzioth.Utilities;
using UnityEngine.Audio;
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

	[SerializeField] AudioSource _boomerangFlyAuSrc;

	[SerializeField] AudioSource _javelinPickupAuSrc, _shieldPickupAuSrc, _staminaBoostAuSrc;
	[SerializeField] AudioSource _ammoPickupAuSrc, _healthPotionAuSrc, _boomerangPickupAuSrc; 
	[SerializeField] AudioSource _whipPickupAuSrc, _slowPickupAuSrc;

	[Header("Enemy SFX")]
	[SerializeField] AudioSource _enemyDeathAuSrc;
	[SerializeField] AudioClip[] _enemyDeathAuClips;
	[SerializeField] AudioSource _enemyImpactAuSrc;
	[SerializeField] AudioSource _zombieCrowdAuSrc, _zombieHurtAuSrc;
	[SerializeField] AudioClip[] _zombieHurtClips;

	[Header("Mixer")]
	[SerializeField] AudioMixer _audioMixer;
	public bool MusicIsMuted { get; private set; } 
	public bool SfxIsMuted { get; private set; }
	const string MUSIC_VOLUME_KEY = "music volume";
	const string SFX_VOLUME_KEY = "sfx volume";
	float _musicVolume, _sfxVolume;

#endregion
#region Base Methods

    protected override void Initialize()
    {
        base.Initialize();

		_musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0f);
		_sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0f);
		if (_musicVolume == -80f) MusicIsMuted = true;
		if (_sfxVolume == -80f) SfxIsMuted = true;
    }
    private void Start()
    {
        _audioMixer.SetFloat("MusicVolume", _musicVolume);
		_audioMixer.SetFloat("SFXVolume", _sfxVolume);
    }
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
	public void Play_PlayerLoseShields()
	{
		_shieldPickupAuSrc.pitch = 0.5f;
		_shieldPickupAuSrc.Play();
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

	public void Play_BoomerangFly()
	{
		_boomerangFlyAuSrc.Play();
	}
	public void Stop_BoomerangFly()
	{
		_boomerangFlyAuSrc.Stop();
	}

	void PlayPowerupPickup(PowerupType powerupType)
	{
		switch (powerupType)
		{
			case PowerupType.Javelin:
				_javelinPickupAuSrc.Play();
				break;
			case PowerupType.Shield:
				_shieldPickupAuSrc.pitch = 1;
				_shieldPickupAuSrc.Play();
				break;
			case PowerupType.StaminaBoost:
				_staminaBoostAuSrc.Play();
				break;
			case PowerupType.Ammo:
				PlayAmmoPickup();
				break;
			case PowerupType.HealthPotion:
				_healthPotionAuSrc.Play();
				break;
			case PowerupType.Boomerang:
				_boomerangPickupAuSrc.Play();
				break;
			case PowerupType.Whip:
				_whipPickupAuSrc.Play();
				break;
			case PowerupType.Negative_Slow:
				_slowPickupAuSrc.Play();
				break;
		}
	}
	void PlayAmmoPickup()
	{
		_ammoPickupAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_ammoPickupAuSrc.Play();
	}

	public void PlayEnemyDeath(EnemyType enemyType)
	{
		switch (enemyType)
		{
			case EnemyType.Huntress:
				_enemyDeathAuSrc.pitch = Random.Range(1.6f, 1.7f);
				_enemyDeathAuSrc.PlayOneShot(RandomClip(_enemyDeathAuClips));
				break;
			case EnemyType.Zombie:
				Play_ZombieHurt();
				break;
			default:
				_enemyDeathAuSrc.pitch = Random.Range(0.9f, 1.1f);
				_enemyDeathAuSrc.PlayOneShot(RandomClip(_enemyDeathAuClips));
				break;
		}
	}

	public void PlayEnemyImpact()
	{
		_enemyImpactAuSrc.Play();
	}

	public void Play_ZombieCrowd()
	{
		_zombieCrowdAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_zombieCrowdAuSrc.Play();
	}
	void Play_ZombieHurt()
	{
		var clip = RandomClip(_zombieHurtClips);
		_zombieHurtAuSrc.pitch = Random.Range(0.9f, 1.1f);
		_zombieHurtAuSrc.PlayOneShot(clip);
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

	public void Toggle_MuteMusic()
	{
		MusicIsMuted = !MusicIsMuted;
		float volume;

		if (MusicIsMuted)
		{
			volume = -80f;
		}
		else 
		{
			volume = 0f;
		}

		_audioMixer.SetFloat("MusicVolume", volume);
		PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
	}
	public void Toggle_MuteSfx()
	{
		SfxIsMuted = !SfxIsMuted;
		float volume;

		if (SfxIsMuted)
		{
			volume = -80f;
		}
		else 
		{
			volume = 0f;
		}

		_audioMixer.SetFloat("SFXVolume", volume);
		PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
	}


}