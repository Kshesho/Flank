#region Using Statements
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] TextMeshProUGUI _killsTxt;

    [SerializeField] TextMeshProUGUI _hpTxt;
    [SerializeField] Image _hpBarImage;

    [SerializeField] TextMeshProUGUI _ammoTxt;

    [SerializeField] Image _staminaBarImage;
    [SerializeField] GameObject _staminaBarBorderImage;
    Color _halfAlpha = new Color(1, 1, 1, 0.5f);

    //Abilities
    [SerializeField] Image _staminaBoostIcon1, _staminaBoostIcon2, _staminaBoostIconBG;
    Color _fadedGreen = new Color(0, 0.502f, 0.09449412f, 1);

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

#endregion

    public void UpdateScoreText(int score)
    {
        _killsTxt.text = "Kills: " + score;
    }

    public void UpdateHealthUI(int curHealth, int totalHealth)
    {
        _hpTxt.text = $"{curHealth} / {totalHealth}";
        float fillAmount = (float)curHealth / (float)totalHealth;
        _hpBarImage.fillAmount = fillAmount;
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

    //Abilities
    public void StaminaBoostIcon_Fade()
    {
        _staminaBoostIconBG.color = Color.gray;
        _staminaBoostIcon1.color = _fadedGreen;
        _staminaBoostIcon2.color = _fadedGreen;
    }
    public void StaminaBoostIcon_Restore()
    {
         _staminaBoostIconBG.color = Color.white;
        _staminaBoostIcon1.color = Color.green;
        _staminaBoostIcon2.color = Color.green;
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


}