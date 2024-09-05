#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles powerup behavior (movemnt and collision)
/// </summary>
public class Powerup : MonoBehaviour 
{
#region Variables

    [SerializeField] PowerupType _powerupType;
    [SerializeField] float _moveSpeed;

#endregion
	
	void Update () 
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime, Space.World);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Events.OnPowerupCollected?.Invoke(_powerupType);
            Destroy(this.gameObject);
        }
    }


}