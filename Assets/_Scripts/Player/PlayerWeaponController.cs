#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using UnityEngine;
#endregion

/// <summary>
/// Handles input and toggling player's weapons.
/// </summary>
public class PlayerWeaponController : MonoBehaviour 
{
#region Variables

    Coroutine _disableJavelinRtn;
    float _javelinActiveTime = 6;

    Weapon _primaryActiveWeapon;
    [SerializeField] Weapon _sword;

    Weapon _secondaryActiveWeapon;
    [SerializeField] Weapon _shurikens, _javelins;

#endregion
#region Base Methods

    void Start()
    {
        _primaryActiveWeapon = _sword;
        _secondaryActiveWeapon = _shurikens;
    }

    void OnEnable()
    {
        Events.OnPowerupCollected += CollectPowerup;
    }
    void OnDisable()
    {
        Events.OnPowerupCollected -= CollectPowerup;
    }

    void Update () 
    {
        if (GameManager.Instance.GamePaused)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _primaryActiveWeapon.Attack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _secondaryActiveWeapon.Attack();
        }
	}

#endregion

    void CollectPowerup(PowerupType powerupType)
    {
        if (powerupType == PowerupType.Javelin) 
        {
            _secondaryActiveWeapon = _javelins;
            if (_disableJavelinRtn != null) StopCoroutine(_disableJavelinRtn);
            _disableJavelinRtn = StartCoroutine(DisableJevelinRtn());
        }
    }
    IEnumerator DisableJevelinRtn()
    {
        yield return HM.WaitTime(_javelinActiveTime);
        _secondaryActiveWeapon = _shurikens;
    }


}