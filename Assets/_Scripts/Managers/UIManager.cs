#region Using Statements
using System.Collections;
using UnityEngine;
using TMPro;
using Narzioth.Utilities;
using UnityEngine.UI;
#endregion

/// <summary>
/// Communicates with primary UI elements.
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
#region Variables

    [SerializeField] WaveUI _waveUI;

    [SerializeField] TextMeshProUGUI _killsTxt, _highScoreText;

    [SerializeField] TextMeshProUGUI _hpTxt;
    [SerializeField] Image _hpBarImage;
    //tail
    [SerializeField] Image _hpTailImage;
    Coroutine _hpTailRtn;
    bool _hpTailRtnRunning;
    bool _hpTailMoving;
    [SerializeField] float _hpTailLerpSpeed = 1;

    [SerializeField] GameObject _shurikenImage, _javelinImage, _boomerangImage;
    [SerializeField] TextMeshProUGUI _ammoTxt, _whipTimerTxt;
    Coroutine _javelinTimerRtn, _whipTimerRtn;
    [SerializeField] GameObject _swordImage, _whipImage;

    [SerializeField] Image _staminaBarImage;
    [SerializeField] GameObject _staminaBarBorderImage;
    Color _halfAlpha = new Color(1, 1, 1, 0.5f);

    //Abilities
    [SerializeField] GameObject _staminaBoostIcon, _slowedIcon, _shieldIcon, _stoppedIcon;

    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] TextMeshProUGUI _newHighScoreTxt;

    [SerializeField] GameObject _pauseMenu;

    [SerializeField] GameObject _endgameCanvasGO;

#endregion
#region Base Methods

    void OnEnable () 
    {
        Events.OnPlayerDeath += GameOverUI;
	}
	
	void OnDisable () 
    {
		Events.OnPlayerDeath -= GameOverUI;
	}

    void Update()
    {
        HealthTailFollow();
    }

#endregion

    /// <summary>
    /// Updates the health bar on the HUD, showing a trailing health bar if <paramref name="curHealth"/> is less than <paramref name="previousHealth"/>.
    /// </summary>
    /// <param name="previousHealth"></param>
    /// <param name="curHealth"></param>
    /// <param name="totalHealth"></param>
    public void UpdateHealthUI(int previousHealth, int curHealth, int totalHealth)
    {
        bool losingHealth = curHealth < previousHealth ? true : false;

        // TODO:
        ///let's say I lose 80 HP
        ///then I gain 50
        ///How do I make sure the lerp doesn't stop until it's caught up with the current fill?

        _hpTxt.text = $"{curHealth} / {totalHealth}";
        float fillAmount = (float)curHealth / (float)totalHealth;
        _hpBarImage.fillAmount = fillAmount;

        if (losingHealth)
        {
            if (!_hpTailRtnRunning)
                _hpTailRtn = StartCoroutine(HpTailRtn());
        }
        else
        {
            if (_hpTailRtn != null) StopCoroutine(_hpTailRtn);
            _hpTailMoving = false;
            _hpTailImage.fillAmount = _hpBarImage.fillAmount - 0.01f;
        }
    }
    IEnumerator HpTailRtn()
    {
        _hpTailRtnRunning = true;

        yield return HM.WaitTime(0.8f);
        _hpTailMoving = true;

        _hpTailRtnRunning = false;
    }
    void HealthTailFollow()
    {
        if (_hpTailMoving)
        {
            if (_hpTailImage.fillAmount > _hpBarImage.fillAmount)
            {
                _hpTailImage.fillAmount -= _hpTailLerpSpeed * Time.deltaTime;
            }
            else
            {
                _hpTailMoving = false;
                _hpTailImage.fillAmount = _hpBarImage.fillAmount - 0.01f;
            }
        }
    }

    /// <summary>
    /// Hard-sets the stamina bar fill based on <paramref name="curStamina"/> and <paramref name="maxStamina"/>.
    /// Best used for when player consumes stamina. 
    /// </summary>
    /// <param name="curStamina"></param>
    /// <param name="maxStamina"></param>
    public void SetStaminaBarFill(float curStaminaRatio)
    {
        _staminaBarImage.fillAmount = curStaminaRatio;
    }
    public void StaminaCooldownVisual_On()
    {
        _staminaBarBorderImage.SetActive(true);
        _staminaBarImage.color = _halfAlpha;
    }
    public void StaminaCooldownVisual_Off()
    {
        _staminaBarBorderImage.SetActive(false);
        _staminaBarImage.color = Color.white;
    }

    public void UpdateAmmoCount(int count)
    {
        _ammoTxt.text = count.ToString();
    }
    public void ChangeWeaponIcon(WeaponType weaponType, int ammo)
    {
        switch (weaponType)
        {
            case WeaponType.Shuriken:
                _javelinImage.SetActive(false);
                _boomerangImage.SetActive(false);
                _shurikenImage.SetActive(true);

                _ammoTxt.text = ammo.ToString();
                break;
            case WeaponType.Javelin:
                _shurikenImage.SetActive(false);
                _boomerangImage.SetActive(false);
                _javelinImage.SetActive(true);

                if (_javelinTimerRtn != null)
                {
                    StopCoroutine(_javelinTimerRtn);
                    //in case the text was turned red by the stopped coroutine
                    _ammoTxt.color = Color.white;
                }
                _javelinTimerRtn = StartCoroutine(JavelinTimerRtn(ammo));

                break;
            case WeaponType.Boomerang:
                _shurikenImage.SetActive(false);
                _javelinImage.SetActive(false);
                _boomerangImage.SetActive(true);

                _ammoTxt.text = ammo.ToString();
                break;
            //----Melee
            case WeaponType.Sword:
                _whipImage.SetActive(false);
                _swordImage.SetActive(true);
                _whipTimerTxt.gameObject.SetActive(false);
                break;
            case WeaponType.Whip:
                _swordImage.SetActive(false);
                _whipImage.SetActive(true);
                _whipTimerTxt.gameObject.SetActive(true);

                if (_whipTimerRtn != null)
                {
                    StopCoroutine(_whipTimerRtn);
                    //in case the text was turned red by the stopped coroutine
                    _whipTimerTxt.color = Color.white;
                }
                _whipTimerRtn = StartCoroutine(WhipTimerRtn(ammo));
                break;
        }
    }
    IEnumerator JavelinTimerRtn(int activeTime)
    {
        for (int i = activeTime; i > 0; i--)
        {
            _ammoTxt.text = $"{i}s";
            if (i < 3)
                _ammoTxt.color = Color.red;

            yield return HM.WaitTime(1);
        }
        _ammoTxt.color = Color.white;
    }
    IEnumerator WhipTimerRtn(int activeTime)
    {
        for (int i = activeTime; i > 0; i--)
        {
            _whipTimerTxt.text = $"{i}s";
            if (i < 3)
                _whipTimerTxt.color = Color.red;

            yield return HM.WaitTime(1);
        }
        _whipTimerTxt.color = Color.white;
    }


    public void UpdateScoreText(int score)
    {
        _killsTxt.text = "Score: " + score;
    }
    public void UpdateHighScoreText(int highScore)
    {
        _highScoreText.text = highScore.ToString();
    }

    #region Buffs

    public void Enable_StaminaBoostIcon()
    {
        _staminaBoostIcon.SetActive(true);
    }
    public void Disable_StaminaBoostIcon()
    {
        _staminaBoostIcon.SetActive(false);
    }

    public void Enable_SlowedIcon()
    {
        _slowedIcon.SetActive(true);
    }
    public void Disable_SlowedIcon()
    {
        _slowedIcon.SetActive(false);
    }

    public void EnableShieldIcon()
    {
        _shieldIcon.SetActive(true);
    }
    public void DisableShieldIcon()
    {
        _shieldIcon.SetActive(false);
    }

    public void EnableStoppedIcon()
    {
        _stoppedIcon.SetActive(true);
    }
    public void DisableStoppedIcon()
    {
        _stoppedIcon.SetActive(false);
    }

    #endregion


    void GameOverUI()
    {
        _gameOverScreen.SetActive(true);
        if (GameManager.Instance.NewHighScoreReached)
        {
            _newHighScoreTxt.text = "New high score!";
        }
    }

    public void EnablePauseMenu()
    {
        _pauseMenu.SetActive(true);
    }
    public void DisablePauseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    //Enemy Wave UI
    public void NewWaveUI(int wave, int WaveTime)
    {
        _waveUI.NewWaveUI(wave, WaveTime);
    }
    public void LastWaveUI()
    {
        _waveUI.LastWave();
    }
    public void UpdateWaveTimer(int time)
    {
        _waveUI.UpdateWaveTimer(time);
    }
    public void NextWaveCountdown(string text)
    {
        _waveUI.NextWaveCountdown(text);
    }

    public void BossUI()
    {
        _waveUI.BossWaveUI();
    }

    public void EndgameSequence()
    {
        //play animation that plays when canvas is set active.
        _endgameCanvasGO.SetActive(true);
    }
    public void EndgameSequenceFinished()
    {
        _endgameCanvasGO.SetActive(false);
        _waveUI.EnableEndgameElements();
    }
    public void UpdateSpawnRate(float newSpawnRate)
    {
        _waveUI.UpdateSpawnRate(newSpawnRate);
    }

}