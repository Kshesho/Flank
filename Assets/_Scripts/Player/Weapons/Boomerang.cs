#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Boomerang : Weapon 
{
#region Variables

    [SerializeField] PlayerWeaponController _wepController;
    [SerializeField] GameObject _boomerangProjectile;
    int _curAmmo, _maxAmmo = 5;
    bool _boomerangAway;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnBoomerangReturned += BoomerangReturned;   
    }
    void OnDisable()
    {
        Events.OnBoomerangReturned += BoomerangReturned;
    }

    #endregion

    public override void Attack()
    {
        if (!_boomerangAway)
        {
            _boomerangAway = true;
            Instantiate(_boomerangProjectile, transform.position, Quaternion.identity);
            PlayerStateManager.Instance.ThrowStarted();
        }
    }

    public void BoomerangReturned(bool enemyHit)
    {
        if (enemyHit)
            _curAmmo--;
        
        if (_curAmmo < 1)
            _wepController.DisableBoomerang();

        _boomerangAway = false;
    }

    public void RefillAmmo()
    {
        _curAmmo = _maxAmmo;
    }

}