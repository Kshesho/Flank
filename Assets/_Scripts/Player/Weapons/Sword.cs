#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class Sword : Weapon 
{
#region Variables

    [SerializeField] PlayerAnimStateChanger _playerAnimChanger;
    [SerializeField] Animator _swordAnim;

#endregion

    void Update()
    {
        var h = _playerAnimChanger.HorizontalInput;
        var v = _playerAnimChanger.VerticalInput;

        //if player isn't moving, that means they're not changing direction, so don't change sword rotation
        if (h == 0 && v == 0)
            return;

        var zRot = 0;

        if (v == 1) //N (and NW, NE)
            zRot = 0;
     
        else if (v == -1) //S (and SW, SE)
            zRot = -180;
        
        else if (v == 0 && h == -1) //W
            zRot = 90;
        
        else if (v == 0 && h == 1) //E
            zRot = -90;
        
        transform.rotation = Quaternion.Euler(0, 0, zRot);
    }

    public override void Attack()
    {
        if (CooldownFinished())
        {
            StartCooldown();
            _swordAnim.SetTrigger("swing");
            PlayerStateManager.Instance.AttackStarted();
            AudioManager.Instance.PlaySwordSwing();
        }
    }


}