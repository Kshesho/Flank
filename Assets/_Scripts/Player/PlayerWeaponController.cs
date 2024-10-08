#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles input and activating player's weapons when appropriate.
/// </summary>
public class PlayerWeaponController : MonoBehaviour 
{
#region Variables

    [SerializeField] PlayerAnimStateChanger _playerAnimChanger;

    bool _ninjaStarActive = true, _javelinActive;

    [SerializeField] GameObject _ninjaStarPref;
    [SerializeField] float _nsCooldownTime = 0.25f;
    float _canThrowNSTime = -1;
    bool NinjaStarCooldownFinished()
    {
        if (_canThrowNSTime < Time.time)
            return true;
        return false;
    }
    Vector3 _spawnOffset = new Vector3(0, 0.72f, 0);

    //----Javelin
    [SerializeField] GameObject _javelinPref;
    [SerializeField] float _javelinCooldownTime = 0.7f;
    float _canThrowJavTime = -1;
    bool JavelinCooldownFinished()
    {
        if (_canThrowJavTime < Time.time)
            return true;
        return false;
    }
    Coroutine _disableJavelinRtn;
    float _javelinActiveTime = 5;


    float _swordCooldownTime = 0.5f;
    [SerializeField] Animator _swordAnim;
    float _canSwingTime = -1;
    bool SwordCooldownFinished()
    {
        if (_canSwingTime < Time.time)
            return true;
        return false;
    }

#endregion
#region Base Methods

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

        if (Input.GetMouseButtonDown(1))
        {
            if (_ninjaStarActive)
                ThrowNinjaStar();
            else if (_javelinActive)
                ThrowJavelin();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (SwordCooldownFinished())
            {
                SwingSword();
            }
        }
	}

#endregion

    void SwingSword()
    {
        _canSwingTime = Time.time + _swordCooldownTime;
        _swordAnim.SetTrigger("swing");
        _playerAnimChanger.Attack();
        AudioManager.Instance.PlaySwordSwing();
    }

    void ThrowNinjaStar()
    {
        if (NinjaStarCooldownFinished())
        {
            Instantiate(_ninjaStarPref, transform.position + _spawnOffset, Quaternion.identity);
            _canThrowNSTime = Time.time + _nsCooldownTime;
        }
    }

    void ThrowJavelin()
    {
        if (JavelinCooldownFinished())
        {
            Instantiate(_javelinPref, transform.position, Quaternion.identity);
            _canThrowJavTime = Time.time + _javelinCooldownTime;
        }
    }


    void CollectPowerup(PowerupType powerupType)
    {
        if (powerupType == PowerupType.Javelin) 
        {
            _ninjaStarActive = false;
            _javelinActive = true;
            if (_disableJavelinRtn != null) StopCoroutine(_disableJavelinRtn);
            _disableJavelinRtn = StartCoroutine(DisableJevelinRtn());
        }
    }
    IEnumerator DisableJevelinRtn()
    {
        yield return HM.WaitTime(_javelinActiveTime);
        _javelinActive = false;
        _ninjaStarActive = true;
    }


}