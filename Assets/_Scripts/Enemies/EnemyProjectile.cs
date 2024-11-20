#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class EnemyProjectile : MonoBehaviour 
{
#region Variables

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] int _damage = 10;

#endregion
#region Base Methods
	
	void Update () 
    {
		transform.Translate(Vector2.up * _moveSpeed * Time.deltaTime, Space.Self);
	}

#endregion

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Events.OnCollide?.Invoke(other, _damage);
            Destroy(this.gameObject);
        }
    }


}