#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles changing player's animation state parameters based on input
/// </summary>
public class PlayerAnimStateChanger : MonoBehaviour 
{
#region Variables

    [SerializeField] Animator _anim;
    bool _dodging;
    

#endregion
#region Base Methods
	
	void Update () 
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        if (_dodging)
            return;

        if (hInput != 0 || vInput != 0)
        {
            _anim.SetBool("moving", true);
            _anim.SetFloat("Horizontal", hInput);
            _anim.SetFloat("Vertical", vInput);
        }
        else
        {
            _anim.SetBool("moving", false);
        }
	}

#endregion

    public void Attack()
    {
        _anim.SetTrigger("attack");
    }

    public void DodgeStarted()
    {
        _dodging = true;
        _anim.SetBool("dodge", true);
    }
    public void DodgeFinished()
    {
        _dodging = false;
        _anim.SetBool("dodge", false);
    }

    public void PlayDamageFlash()
    {
        _anim.Play("Damaged_flash");
    }

    public void Hit()
    {
        _anim.SetTrigger("hit");
    }


}