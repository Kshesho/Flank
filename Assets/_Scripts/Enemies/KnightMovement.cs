#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class KnightMovement : EnemyMovement 
{
#region Variables

	[SerializeField] KnightWeapon _weapon;
	[SerializeField] KnightAnimStateChanger _animStateChanger;

	bool _moving = true;
	float _stopTimer;
	float _averageStopTime = 4;

#endregion
#region Base Methods
	
	protected override void Start()
	{
		base.Start();

		MoveToSpawnPosition();
		RefreshStopTimer();
	}
	protected override void Update () 
    {
		base.Update();

		if (_stopTimer < Time.time && _moving)
		{
			StopMoving();
			_weapon.Attack();
		}
	}

#endregion

	// make sure I'm on-screen (between y bounds) before attacking

	//called via keyframe in 'Knight_charge_color blink' animation
	void ResumeMovement()
	{
		_animStateChanger.StopAttack();
		_moveSpeed = _baseMoveSpeed;
		_moving = true;
		RefreshStopTimer();
	}

	void RefreshStopTimer()
	{
		_stopTimer = Time.time + Random.Range(_averageStopTime - 1f, _averageStopTime + 1f);
	}

	public override void StopMoving()
	{
		base.StopMoving();
		_moving = false;
	}

}