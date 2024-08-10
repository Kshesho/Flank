#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class PlayerMovement : MonoBehaviour 
{
#region Variables

    [SerializeField] Vector2 _startPos;
    [SerializeField] float _moveSpeed = 3f;

    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;

#endregion
#region Base Methods

    void Awake()
    {
        
    }

    void Start() 
    {
        transform.position = _startPos;
	}
	
	void FixedUpdate() 
    {
        Move();
        ConstrainPosition();
	}

#endregion

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        dir.Normalize();
        transform.Translate(dir * _moveSpeed * Time.fixedDeltaTime);
    }

    void ConstrainPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, (_xBounds * -1), _xBounds);
        float clampedY = Mathf.Clamp(transform.position.y, (_yBounds * -1), _yBounds);
        transform.position = new Vector2(clampedX, clampedY);
    }

}