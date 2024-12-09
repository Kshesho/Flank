#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles animations for the Enemy
/// </summary>
public class EnemyAnimStateChanger : MonoBehaviour 
{
#region Variables

    [SerializeField] protected Animator _anim;
    [SerializeField] protected AnimationClip _animDeathClip;

#endregion

    public float DeathAnimClipLength()
    {
        return _animDeathClip.length;
    }
    public void PlayDeathAnimation()
    {
        _anim.SetTrigger("death");
    }

}