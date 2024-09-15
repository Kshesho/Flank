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

    [SerializeField] TextMeshProUGUI _scoreTxt;

    [SerializeField] TextMeshProUGUI _hpTxt;
    [SerializeField] Image _hpBarImage;

    [SerializeField] Image _staminaBarImage;

#endregion
#region Base Methods

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
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
    /// Lerps the stamina bar fill from <paramref name="curStamina"/> to <paramref name="maxStamina"/>.
    /// </summary>
    /// <param name="curStamina"></param>
    /// <param name="maxStamina"></param>
    public void RefillStaminaBar(int curStamina, int maxStamina, float staminaGainPerSecond)
    {
        float newFill = (float)curStamina / (float)maxStamina;
        //convert stamina/s to 0-1 fill amount
        float fillSpeed = staminaGainPerSecond * 0.1f;
        float lerpedFill = Mathf.Lerp(_staminaBarImage.fillAmount, newFill, fillSpeed * Time.deltaTime);
        // Do I need to check if the fill amount is less than current, so it doesn't lerp backwards?
        _staminaBarImage.fillAmount = lerpedFill;
        //Set stamina text (convert float to int)
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


}