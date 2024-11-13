#region Using Statements
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#endregion

/// <summary>
/// Updates the UI elements that are associated with enemy waves.
/// </summary>
public class WaveUI : MonoBehaviour 
{
#region Variables

    [SerializeField] TextMeshProUGUI _waveNumTxt, _waveTimeTxt;
    [SerializeField] Animator _waveTimeTxtAnim;

#endregion

    public void NewWaveUI(int wave, int WaveTime)
    {
        ResetWaveTimerText();
        _waveNumTxt.text = wave.ToString();
        _waveTimeTxt.text = FormattedTime(WaveTime);
    }
    public void UpdateWaveTimer(int time)
    {
        _waveTimeTxt.text = FormattedTime(time);
        //if less than 5 seconds, turn red
        if (time <= 5)
        {
            _waveTimeTxtAnim.SetTrigger("shrink");
            _waveTimeTxt.color = Color.red;
            _waveTimeTxt.fontSize = 36;
        }
        else ResetWaveTimerText();
    }
    /// <summary>
    /// Changes the wave timer text for the countdown between waves.
    /// </summary>
    /// <param name="text"></param>
    public void NextWaveCountdown(string text)
    {
        _waveTimeTxt.text = text;
        _waveTimeTxt.color = Color.gray;
        _waveTimeTxt.fontSize = 36;
    }

    string FormattedTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ResetWaveTimerText()
    {
        _waveTimeTxt.color = Color.white;
        _waveTimeTxt.fontSize= 30;
    }


}