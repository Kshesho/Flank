#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Keeps my position within X and Y boundaries.
/// </summary>
public class ClampPosition : MonoBehaviour 
{
#region Variables

	[SerializeField] Vector2 _xBounds;
	[SerializeField] Vector2 _yBounds;

#endregion
#region Base Methods
	
	void Update () 
    {
		float clampedX = Mathf.Clamp(transform.position.x, _xBounds.x, _xBounds.y);
		float clampedY = Mathf.Clamp(transform.position.y, _yBounds.x, _yBounds.y);
		transform.position = new Vector2(clampedX, clampedY);
	}

#endregion

}