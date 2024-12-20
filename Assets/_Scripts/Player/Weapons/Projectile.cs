#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles behavior for projectiles
/// </summary>
public class Projectile : MonoBehaviour 
{
#region Variables

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] int _damage = 10;

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
        transform.Translate(Vector2.up * _moveSpeed * Time.deltaTime, Space.Self);
	}

#endregion

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, _damage);
            AudioManager.Instance.PlayEnemyImpact();
            Destroy(this.gameObject, 0.01f); //this delay is a sloppy workaround. For some reason, without it, this was being destroyed before the line above could run
        }
    }
    
    
}