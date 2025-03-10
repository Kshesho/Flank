#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

/// <summary>
/// Controls damage and health for the boss
/// </summary>
public class BossHeart : MonoBehaviour 
{
#region Variables

    [SerializeField] Image _healthBarImg;
    GameObject _healthCanvas;

    [SerializeField] BossFight _bossFight;
    bool _blocking, _floating;

    int _curHealth, _maxHealth = 1000;

#endregion
#region Base Methods

    private void Start()
    {
        _healthCanvas = _healthBarImg.transform.parent.gameObject;
        _curHealth = _maxHealth;
    }
    private void OnEnable()
    {
        Events.OnBossCollide_Projectile += CollideWith_PlayerProjectile;
        Events.OnBossCollide_Melee += CollideWith_PlayerMeleeWeapon;
    }
    private void OnDisable()
    {
        Events.OnBossCollide_Projectile -= CollideWith_PlayerProjectile;
        Events.OnBossCollide_Melee -= CollideWith_PlayerMeleeWeapon;
    }

#endregion

    void CollideWith_PlayerMeleeWeapon(int damage)
    {
        if (!_blocking && !_floating)
            TakeDamage(damage);
    }

    void CollideWith_PlayerProjectile(int damage, GameObject projectile)
    {
        bool projIsBoomerang = projectile.name.Contains("Boomerang") ? true : false;

        if (_blocking)
        {
            if (!projIsBoomerang) Destroy(projectile);
        }
        else if (!_floating)
        {
            //Don't destroy the Boomerang since it's not a normal projectile
            if (!projIsBoomerang) Destroy(projectile);
            TakeDamage(damage);
        }
    }

    void TakeDamage(int damage_)
    {
        _curHealth -= damage_;

        if (_curHealth < 1)
        {
            _healthCanvas.SetActive(false);
            _bossFight.ChangeState(BossState.Dying);
        }
        else
        {
            float fill = (float)_curHealth / (float)_maxHealth;
            _healthBarImg.fillAmount = fill;
        }
    }

    public void Floating_On()
    {
        _floating = true;
        _healthCanvas.SetActive(false);
    }
    public void Floating_Off()
    {
        _floating = false;
        _healthCanvas.SetActive(true);
    }
    public void Blocking_On()
    {
        _blocking = true;
    }
    public void Blocking_Off()
    {
        _blocking = false;
    }

}