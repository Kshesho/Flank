#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;
#endregion

/// <summary>
/// Turns the powerup magnet on and off.
/// </summary>
public class PowerupMagnetizer : MonoBehaviour 
{
#region Variables

    [SerializeField] GameObject _rangeSprite;

#endregion
#region Base Methods
	
	void Update () 
    {
		if (Input.GetKeyDown(KeyCode.C))
        {
            TurnOnMagnet();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            TurnOffMagnet();
        }
	}
    
#endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Powerup))
        {
            Events.OnPowerupMagnetized?.Invoke(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Powerup))
        {
            Events.OnPowerupUnmagnetized?.Invoke(other.gameObject);
        }
    }

    void TurnOnMagnet()
    {
        _rangeSprite.SetActive(true);
    }
    void TurnOffMagnet()
    {
        _rangeSprite.SetActive(false);
    }

}