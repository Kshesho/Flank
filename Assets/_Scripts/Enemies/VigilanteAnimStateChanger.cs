#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class VigilanteAnimStateChanger : EnemyAnimStateChanger 
{
#region Variables



#endregion
#region Base Methods

#endregion

    public void ChangeRunAnim(bool runLeft)
    {
        _anim.SetBool("runLeft", runLeft);
    }

    public void PlayThrow()
    {
        FacePlayer();
        _anim.SetTrigger("throw");
    }

}