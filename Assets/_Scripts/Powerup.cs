#region Using Statements
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
    [Tooltip("How many hits until an enemy destroys me.")]
    [SerializeField] int _health = 1;
    [SerializeField] Collider2D _thisCollider;

#endregion

    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
    }

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

    void TakeDamage(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider)
            return;

        _health -= damage;
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}