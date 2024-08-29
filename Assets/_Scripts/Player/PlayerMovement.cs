#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles player movement 
/// </summary>
public class PlayerMovement : MonoBehaviour 
{
#region Variables

    [SerializeField] Rigidbody2D _rBody;
    [SerializeField] Vector2 _startPos;
    [SerializeField] float _moveSpeed = 3f;
    float _xInput, _yInput;
    Vector2 _dir = Vector2.zero;

    [SerializeField] float _xBounds = 9.37f, _yBounds = 5;

#endregion
#region Base Methods

    void Start() 
    {
        transform.position = _startPos;
	}

    void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {
        Move();
        ConstrainPosition();
	}

#endregion

    void Move()
    {
        //Vector2 dir = new Vector2(_xInput, _yInput);
        _dir.x = _xInput;
        _dir.y = _yInput;
        _dir.Normalize();
        //transform.Translate(dir * _moveSpeed * Time.fixedDeltaTime);
        _rBody.velocity = _dir * _moveSpeed * Time.fixedDeltaTime;
    }

    void ConstrainPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, (_xBounds * -1), _xBounds);
        float clampedY = Mathf.Clamp(transform.position.y, ((_yBounds + 0.65f) * -1), _yBounds);
        transform.position = new Vector2(clampedX, clampedY);
    }

}