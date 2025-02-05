#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles the attack for the Vigilante.
/// </summary>
public class VigilanteAttack : MonoBehaviour 
{
#region Variables

    [SerializeField] VigilanteAnimStateChanger _animStateChanger;
    [SerializeField] GameObject _knifePref;
    float _cooldownTimer;
    [SerializeField] Vector2 _throwCooldownMinMax = new Vector2(2f, 3.5f);
    bool _dying;

#endregion
#region Base Methods

    void Start () 
    {
        TriggerCooldown();	
	}
    private void Update()
    {
        if (!_dying) ThrowKnife();        
    }

#endregion

    void ThrowKnife()
    {
        if (_cooldownTimer <= Time.time)
        {
            TriggerCooldown();

            /*create Knife*/ 
            Instantiate(_knifePref, transform.position, Quaternion.identity);

            _animStateChanger.PlayThrow();
        }
    }

    void TriggerCooldown()
    {
        var rand = Random.Range(_throwCooldownMinMax.x, _throwCooldownMinMax.y);
        _cooldownTimer = Time.time + rand;
    }

    public void StopAttacking()
    {
        _dying = true;
    }

}