#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles the Javelin weapon's behavior
/// </summary>
public class Javelin : MonoBehaviour 
{
#region Variables

    [SerializeField] int _damage = 10;
    [SerializeField] float _moveSpeed = 3;

    #endregion
#region Base Methods

    void Start()
    {
        //correct the rotation due to a rotated sprite
        transform.rotation = Quaternion.Euler(0, 0, 45);
    }
    void Update()
    {
        transform.Translate(Vector2.up * _moveSpeed *  Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, _damage);
        }
    }

#endregion



}