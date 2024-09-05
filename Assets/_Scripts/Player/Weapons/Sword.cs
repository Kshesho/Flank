#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles the collision for the sword weapon
/// </summary>
public class Sword : MonoBehaviour 
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.EDamagable))
        {
            Events.OnCollide?.Invoke(other, 20);
        }
    }


}