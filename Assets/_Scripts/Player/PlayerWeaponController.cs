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

    Coroutine _disableJavelinRtn, _disableWhipRtn;
    [Tooltip("How long the javelin stays active after the player collects the javelin powerup.")]
    [SerializeField] int _javelinActiveTime = 6;
    [Tooltip("How long the whip stays active after the player collects the whip powerup.")]
    [SerializeField] int _whipActiveTime = 15;

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
        _primaryActiveWeapon = _sword;
        _secondaryActiveWeapon = _shurikens;
    }

    void OnEnable()
    {
        Events.OnPowerupCollected += CollectPowerup;
        Events.OnPlayerDeath += TurnOffAllWeapons;
    }
    void OnDisable()
    {
        Events.OnPowerupCollected -= CollectPowerup;
        Events.OnPlayerDeath -= TurnOffAllWeapons;
    }

    void Update () 
    {
        if (!GameManager.Instance.GamePaused)
        {
            HandleAttackInput();
        }
	}

#endregion

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && PlayerStateManager.Instance.PlayerCanPrimaryAttack())
        {
            _primaryActiveWeapon.Attack();
        }

        if (Input.GetMouseButtonDown(1) && PlayerStateManager.Instance.PlayerCanSecondaryAttack())
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
        else if (powerupType == PowerupType.Whip)
        {
            _primaryActiveWeapon = _whip;
            //show weapon icon on the HUD?
            if (_disableWhipRtn != null) StopCoroutine(_disableWhipRtn);
            _disableWhipRtn = StartCoroutine(DisableWhipRtn());
        }
    }
    IEnumerator DisableJevelinRtn()
    {
        yield return HM.WaitTime(_javelinActiveTime);
        _secondaryActiveWeapon = _shurikens;
        // TDOD: instead of changing back to Shuriken, change back to whichever weapon is equipped
        UIManager.Instance.ChangeWeaponIcon(RangedWeaponType.Shuriken, _shurikensSpecificReference.Ammo);
    }
    IEnumerator DisableWhipRtn()
    {
        yield return HM.WaitTime(_whipActiveTime);
        _primaryActiveWeapon = _sword;
    }

    /// <summary>
    /// Disables this gameObject and all of its child weapons. Called when OnPlayerDeath is raised.
    /// </summary>
    void TurnOffAllWeapons()
    {
        this.gameObject.SetActive(false);
    }


}