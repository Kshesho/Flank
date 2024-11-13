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

    [SerializeField] TextMeshProUGUI _killsTxt;

    [SerializeField] TextMeshProUGUI _hpTxt;
    [SerializeField] Image _hpBarImage;
    //tail
    [SerializeField] Image _hpTailImage;
    Coroutine _hpTailRtn;
    bool _hpTailRtnRunning;
    bool _hpTailMoving;
    [SerializeField] float _hpTailLerpSpeed = 1;

    [SerializeField] GameObject _shurikenImage, _javelinImage;
    [SerializeField] TextMeshProUGUI _ammoTxt;
    Coroutine _timerRtn;

    [SerializeField] Image _staminaBarImage;
    [SerializeField] GameObject _staminaBarBorderImage;
    Color _halfAlpha = new Color(1, 1, 1, 0.5f);

    //Abilities
    [SerializeField] Image _staminaBoostIcon1, _staminaBoostIcon2, _staminaBoostIconBG;
    Color _green = new Color(0, 0.502f, 0.09449412f, 1);
    Color _fadedGreen = new Color(0, 0.502f, 0.09449412f, 0.33f);
    Color _tenthAlpha = new Color(1, 1, 1, 0.1f);

    [SerializeField] GameObject _gameOverScreen;

    [SerializeField] GameObject _pauseMenu;

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

        yield return HM.WaitTime(1.1f);
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
    public void ChangeWeaponIcon(RangedWeaponType weaponType, int ammo)
    {
        switch (weaponType)
        {
            case RangedWeaponType.Shuriken:
                _javelinImage.SetActive(false);
                _shurikenImage.SetActive(true);

                _ammoTxt.text = ammo.ToString();
                break;
            case RangedWeaponType.Javelin:
                _shurikenImage.SetActive(false);
                _javelinImage.SetActive(true);

                if (_timerRtn != null)
                {
                    StopCoroutine(_timerRtn);
                    //in case the text was turned red by the stopped coroutine
                    _ammoTxt.color = Color.white;
                }
                _timerRtn = StartCoroutine(TimerRtn(ammo));

                break;
        }
    }
    IEnumerator TimerRtn(int activeTime)
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


    public void UpdateScoreText(int score)
    {
        _killsTxt.text = "Kills: " + score;
    }

    //Abilities
    public void StaminaBoostIcon_Fade()
    {
        _staminaBoostIconBG.color = _tenthAlpha;
        _staminaBoostIcon1.color = _fadedGreen;
        _staminaBoostIcon2.color = _fadedGreen;
    }
    public void StaminaBoostIcon_Restore()
    {
         _staminaBoostIconBG.color = Color.white;
        _staminaBoostIcon1.color = _green;
        _staminaBoostIcon2.color = _green;
    }


    void GameOverUI()
    {
        _gameOverScreen.SetActive(true);
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
    public void UpdateWaveTimer(int time)
    {
        _waveUI.UpdateWaveTimer(time);
    }
    public void NextWaveCountdown(string text)
    {
        _waveUI.NextWaveCountdown(text);
    }

}