using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimStateChanger : EnemyAnimStateChanger
{
#region Variables & Base Methods

    bool _attacking;

    void Update()
    {
        if (_attacking)
        {
            FacePlayer();
        }
    }

#endregion

    public void PlayAttack()
	{
		_attacking = true;
		ChargeUp();
	}
	public void StopAttack()
	{
		_attacking = false;
	}

	void ChargeUp()
    {
        //these animations line up with the same duration, ensuring keyframes are triggered at the right times
		_anim.SetTrigger("charge");
		_anim.Play("Knight_charge_color blink");
    }

}
