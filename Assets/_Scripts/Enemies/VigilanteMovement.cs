#region Using Statements
using Narzioth.Utilities;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class VigilanteMovement : EnemyMovement 
{
#region Variables

	[SerializeField] VigilanteAnimStateChanger _animStateChanger;
	[SerializeField] VigilanteAttack _vigilanteAttack;

	bool _movingLeft;
	bool _dying;
	[Tooltip("How quickly the enemy goes from max speed to 0 upon death")]
	[SerializeField] float _slowdownRate = 1;

#endregion
#region Base Methods

    protected override void Start()
    {
        base.Start();
		_movingLeft.CoinFlip();
		_animStateChanger.ChangeRunAnim(_movingLeft);
		MoveToSpawnPosition();
    }
    protected override void Update () 
    {
		HorizontalMovement();

		if (_dying)
		{
			if (_moveSpeed > 0)
				_moveSpeed = Mathf.Lerp(_moveSpeed, 0, _slowdownRate * Time.deltaTime);
			
			else if (_moveSpeed < 0)
				_moveSpeed = 0;
		}
	}

#endregion

	protected override void MoveToSpawnPosition()
	{
		var randY = Random.Range(-5.24f, 3.95f);

		if (_movingLeft)
			transform.position = new Vector2(_xBounds, randY);
		else transform.position = new Vector2(_xBounds * -1, randY);
	}

	void HorizontalMovement()
	{
		if (_movingLeft)
			transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime, Space.World);
		else
			transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime, Space.World);

		ResetPosition();
	}

    protected override void ResetPosition()
    {
        if (_movingLeft && transform.position.x < _xBounds * -1)
		{
			var randY = Random.Range(-5.24f, 3.95f);

			transform.position = new Vector2(_xBounds, randY);
        }
		else if (!_movingLeft && transform.position.x > _xBounds)
		{
			var randY = Random.Range(-5.24f, 3.95f);

			transform.position = new Vector2(_xBounds * -1, randY);
		}
    }

    public override void StopMoving()
    {
		_vigilanteAttack.StopAttacking();
        _dying = true;
    }

}