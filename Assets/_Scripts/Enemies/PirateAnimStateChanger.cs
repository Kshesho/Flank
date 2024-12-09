using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAnimStateChanger : EnemyAnimStateChanger
{
    /// <summary>
    /// 0 = S, -1 = SW, 1 = SE
    /// </summary>
    /// <param name="direction"></param>
    public void ChageRunDirection(float direction)
    {
        _anim.SetFloat("Diagonal", direction);
    }

}
