#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class EnemyMovement : MonoBehaviour 
{
#region Variables

    [SerializeField] float _moveSpeed = 4;
    [SerializeField] float _yOffScreenPoint, _yResetPoint, _xBounds;

#endregion
#region Base Methods

    void Awake()
    {
        
    }

    void Start () 
    {
		
	}
	
	void Update () 
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime, Space.World);

        if (transform.position.y < _yOffScreenPoint)
        {
            var rand = Random.Range(_xBounds * -1, _xBounds);
            transform.position = new Vector2(rand, _yResetPoint);
        }
	}

#endregion

    public void StopMoving()
    {
        _moveSpeed = 0;
    }


}