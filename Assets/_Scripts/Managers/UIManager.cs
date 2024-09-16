#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Narzioth.Utilities;
using UnityEngine.UI;
using UnityEditor.PackageManager;
#endregion

/// <summary>
/// Communicates with primary UI elements.
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
#region Variables

    [SerializeField] TextMeshProUGUI _scoreTxt;

    [SerializeField] TextMeshProUGUI _hpTxt;
    [SerializeField] Image _hpBarImage;

    [SerializeField] Image _staminaBarImage;

    [SerializeField] GameObject _gameOverScreen;

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
        _scoreTxt.text = "Kills: " + score;
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

    void GameOverUI()
    {
        _gameOverScreen.SetActive(true);
    }


}