#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Turns the powerup magnet on and off.
/// </summary>
public class PowerupMagnetizer : MonoBehaviour 
{
#region Variables

    [SerializeField] GameObject _rangeSprite;
    bool _magnetOn;

#endregion
#region Base Methods
	
	void Update () 
    {
		if (Input.GetKeyDown(KeyCode.Semicolon) 
            || Input.GetKeyDown(KeyCode.LeftAlt) 
            || Input.GetKeyDown(KeyCode.RightAlt))
        {
            TurnOnMagnet();
        }
        else if (_magnetOn && !InputActive())
        {
            TurnOffMagnet();
        }
	}

    #endregion

    bool InputActive()
    {
        if (Input.GetKey(KeyCode.Semicolon)
            || Input.GetKey(KeyCode.LeftAlt)
            || Input.GetKey(KeyCode.RightAlt))
        { 
            return true; 
        }
        return false;
    }
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
        _magnetOn = true;
        _rangeSprite.SetActive(true);
    }
    void TurnOffMagnet()
    {
        _magnetOn = false;
        _rangeSprite.SetActive(false);
    }

}