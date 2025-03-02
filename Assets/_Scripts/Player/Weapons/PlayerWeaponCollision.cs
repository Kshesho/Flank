#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles the collision for player's weapons.
/// </summary>
public class PlayerWeaponCollision : MonoBehaviour 
{
    [SerializeField] int _damage = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, _damage);
        }
    }


}