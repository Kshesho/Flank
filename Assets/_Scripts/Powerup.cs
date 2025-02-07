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
    float _magnetSpeed = 1.75f;
    [Tooltip("How many hits until an enemy destroys me.")]
    [SerializeField] int _health = 1;
    [SerializeField] Collider2D _thisCollider;

    bool _moveTowardsPlayer;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnCollide += TakeDamage;
        Events.OnPowerupMagnetized += MoveTowardsPlayer;
        Events.OnPowerupUnmagnetized += StopMovingTowardsPlayer;
    }
    private void OnDisable()
    {
        Events.OnCollide -= TakeDamage;
        Events.OnPowerupMagnetized -= MoveTowardsPlayer;
        Events.OnPowerupUnmagnetized -= StopMovingTowardsPlayer;
    }

    void Update () 
    {
        if (_moveTowardsPlayer)
        {
            Vector2 dir = GameManager.Instance.PlayerTransform().position - transform.position;
            transform.Translate(dir * _magnetSpeed * Time.deltaTime, Space.World);
        }
        else
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

#endregion

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

    void MoveTowardsPlayer(GameObject goBeingMagnetized)
    {
        if (goBeingMagnetized != this.gameObject)
            return;

        _moveTowardsPlayer = true;
    }
    void StopMovingTowardsPlayer(GameObject goBeingMagnetized)
    {
        if (goBeingMagnetized != this.gameObject)
            return;

        _moveTowardsPlayer = false;
    }


}