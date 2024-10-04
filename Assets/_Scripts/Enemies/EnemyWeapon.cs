#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Collides with and damages player and damagables
/// </summary>
public class EnemyWeapon : MonoBehaviour 
{
#region Variables

    [SerializeField] int _damage = 2;

#endregion
#region Base Methods

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Events.OnCollide?.Invoke(other, _damage);
        }
    }

#endregion


}