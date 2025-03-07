#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class BossKeyframeEvents : MonoBehaviour 
{
    #region Variables

    [SerializeField] BossFight _bossFight;

#endregion

    /// <summary>
    /// Called via Animator keyframe in Boss_Attack_<Direction> animations
    /// </summary>
    public void AttackFinished()
    {
        _bossFight.AttackFinished();
    }

}