#region Using Statements
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
        if (GameManager.Instance.GamePaused)
            return;

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

    public void Throw()
    {
        _anim.SetTrigger("throw");
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

    public void SprintStarted()
    {
        _anim.SetBool("sprint", true);
    }
    public void SprintStopped()
    {
        _anim.SetBool("sprint", false);
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