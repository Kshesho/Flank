#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// Handles animations for the Enemy
/// </summary>
public class EnemyAnimStateChanger : MonoBehaviour 
{
#region Variables

    [SerializeField] Animator _anim;
    [SerializeField] AnimationClip _animDeathClip;

#endregion

    public float DeathAnimClipLength()
    {
        return _animDeathClip.length;
    }
    public void PlayDeathAnimation()
    {
        _anim.SetTrigger("death");
    }

    /// <summary>
    /// 0 = S, -1 = SW, 1 = SE
    /// </summary>
    /// <param name="direction"></param>
    public void ChageRunDirection(float direction)
    {
        _anim.SetFloat("Diagonal", direction);

    }

}