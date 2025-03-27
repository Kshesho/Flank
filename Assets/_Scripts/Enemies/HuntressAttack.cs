#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles throwing the Huntress' net.
/// </summary>
public class HuntressAttack : MonoBehaviour 
{
#region Variables

    [SerializeField] Animator _anim;
    [SerializeField] GameObject _netPref;
    [SerializeField] float _throwCooldown = 4;
    float _throwCooldownTimer;

#endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            if (_throwCooldownTimer < Time.time)
            {
                _throwCooldownTimer = Time.time + _throwCooldown;
                Instantiate(_netPref, transform.position, Quaternion.identity);
                _anim.SetTrigger("throw");
            }
        }
    }

}