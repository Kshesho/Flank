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

    /// <summary>
    /// Changes Horizontal and Vertical animation parameters to play the correct charge animation that faces the player.
    /// </summary>
    void FacePlayer()
	{
		Vector2 pPos = GameManager.Instance.PlayerTransform().position;
		Vector2 facePlayerDir = new Vector2(pPos.x - this.transform.position.x, pPos.y - this.transform.position.y);

		//set values to either 0, 1, or -1
		if (facePlayerDir.x <= -0.5f)
			facePlayerDir.x = -1;
		else if (facePlayerDir.x >= 0.5f) 
			facePlayerDir.x = 1;
		else facePlayerDir.x = 0;

		if (facePlayerDir.y <= -0.5f)
			facePlayerDir.y = -1;
		else if (facePlayerDir.y >= 0.5f) 
			facePlayerDir.y = 1;
		else facePlayerDir.y = 0;

		_anim.SetFloat("Horizontal", facePlayerDir.x);
		_anim.SetFloat("Vertical", facePlayerDir.y);
	}

}
