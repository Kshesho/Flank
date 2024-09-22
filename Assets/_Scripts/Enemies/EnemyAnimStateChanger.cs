#region Using Statements
using System.Collections;
using System.Collections.Generic;
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

}