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

}