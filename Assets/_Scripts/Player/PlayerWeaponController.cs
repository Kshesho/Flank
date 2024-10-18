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
    int _javelinActiveTime = 6;

    Weapon _primaryActiveWeapon;
    [SerializeField] Weapon _sword, _whip;

    Weapon _secondaryActiveWeapon;
    [SerializeField] Weapon _shurikens, _javelins;
    // TODO: when using GetComponent again, remove this reference. OR have a different base type for ranged weapons
    [SerializeField] Shurikens _shurikensSpecificReference;

#endregion
#region Base Methods

    void Start()
    {
        _primaryActiveWeapon = _whip;
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
        if (!GameManager.Instance.GamePaused && 
            !PlayerStateManager.Instance.PlayerIsBusy())
        {
            HandleAttackInput();
        }
	}

#endregion

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _primaryActiveWeapon.Attack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _secondaryActiveWeapon.Attack();
        }
    }

    void CollectPowerup(PowerupType powerupType)
    {
        if (powerupType == PowerupType.Javelin) 
        {
            _secondaryActiveWeapon = _javelins;
            UIManager.Instance.ChangeWeaponIcon(RangedWeaponType.Javelin, _javelinActiveTime);
            if (_disableJavelinRtn != null) StopCoroutine(_disableJavelinRtn);
            _disableJavelinRtn = StartCoroutine(DisableJevelinRtn());
        }
    }
    IEnumerator DisableJevelinRtn()
    {
        yield return HM.WaitTime(_javelinActiveTime);
        _secondaryActiveWeapon = _shurikens;
        // TDOD: instead of changing back to Shuriken, change back to whichever weapon is equipped
        UIManager.Instance.ChangeWeaponIcon(RangedWeaponType.Shuriken, _shurikensSpecificReference.Ammo);
    }


}