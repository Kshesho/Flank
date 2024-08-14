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
    [SerializeField] int _damage;

    WaitForEndOfFrame waitAFrame = new WaitForEndOfFrame();

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

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.EDamagable)
        {
            StartCoroutine(WaitAFrame_ThenCallCollideEvent());
        }
    }
    
    /// <summary>
    /// Waits 1 frame before calling the OnCollide event so that subscribing methods have time to 
    /// subscribe before the event is invoked.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAFrame_ThenCallCollideEvent()
    {
        yield return waitAFrame;
        Events.OnCollide?.Invoke(_damage);
        Destroy(this.gameObject);
    }
    
}