#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Defines the Shurikens thrown weapon behavior.
/// </summary>
public class Shurikens : Weapon 
{

#region Variables

    [SerializeField] GameObject _shurikenProjectilePref;
    Vector3 _spawnOffset = new Vector3(0, 0.72f, 0);

    int _ammo = 15;
    public int Ammo { get { return _ammo; } }

#endregion

    private void OnEnable()
    {
        Events.OnPowerupCollected += FillAmmo;
    }
    private void OnDisable()
    {
        Events.OnPowerupCollected -= FillAmmo;
    }

    public override void Attack()
    {
        if (CooldownFinished())
        {
            if (_ammo > 0)
            {
                DeductAmmo();
                Instantiate(_shurikenProjectilePref, transform.position + _spawnOffset, Quaternion.identity);
                StartCooldown();
                PlayerStateManager.Instance.ThrowStarted();
                //sound effect
            }
        }
    }

    public void FillAmmo(PowerupType powerupType)
    {
        if (powerupType == PowerupType.Ammo)
        {
            _ammo = 15;
            UIManager.Instance.UpdateAmmoCount(_ammo);
        }
    }
    void DeductAmmo()
    {
        _ammo--;
        UIManager.Instance.UpdateAmmoCount(_ammo);
    }


}