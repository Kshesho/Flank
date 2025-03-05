#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class ZombieAnimStateChanger : EnemyAnimStateChanger 
{
#region Variables



#endregion
#region Base Methods

    void Awake()
    {
        
    }

    void Start () 
    {
        RandomizeAnimStartDirection();
	}
	
	void Update () 
    {
		
	}

#endregion

    public void Attack()
    {
        _anim.SetTrigger("attack");
    }
    public void Start_WaitingToAttack()
    {
        _anim.SetBool("waitingToAttack", true);
    }
    public void Stop_WaitingToAttack()
    {
        _anim.SetBool("waitingToAttack", false);
    }

    /// <summary>
    /// Randomize the animation start direction to left, right, up, or down.
    /// </summary>
    void RandomizeAnimStartDirection()
    {
        int rand = Random.Range(0, 4);
        Vector2 dir;

        switch (rand)
        {
            case 1:
                dir = Vector2.left;
                break;
            case 2:
                dir = Vector2.right;
                break;
            case 3:
                dir = Vector2.up;
                break;
            default:
                dir = Vector2.down;
                break;
        }

        _anim.SetFloat("Horizontal", dir.x);
        _anim.SetFloat("Vertical", dir.y);
    }

    /// <summary>
    /// Changes the animation direction parameters to play animations that face in the player's direction.
    /// </summary>
    /// <param name="dirToPlayer"></param>
    public void ChangeAnimDirection(Vector2 dirToPlayer)
    {
        _anim.SetFloat("Horizontal", dirToPlayer.x);
        _anim.SetFloat("Vertical", dirToPlayer.y);
    }

}