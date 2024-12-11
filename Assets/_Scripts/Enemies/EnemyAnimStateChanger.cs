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

    /// <summary>
    /// Sets "Horizontal" and "Vertical" Animator float values to play blend tree animations 
    /// that face the Player's current position.
    /// </summary>
    protected void FacePlayer()
    {
        Vector2 pPos = GameManager.Instance.PlayerTransform().position;
		Vector2 facePlayerDir = new Vector2(pPos.x - this.transform.position.x, pPos.y - this.transform.position.y);

		//set values to either 0, 1, or -1
		if (facePlayerDir.x <= -0.5f)
			facePlayerDir.x = -1;
		else if (facePlayerDir.x >= 0.5f) 
			facePlayerDir.x = 1;
		else facePlayerDir.x = 0;

		if (facePlayerDir.y <= -0.5f)
			facePlayerDir.y = -1;
		else if (facePlayerDir.y >= 0.5f) 
			facePlayerDir.y = 1;
		else facePlayerDir.y = 0;

		_anim.SetFloat("Horizontal", facePlayerDir.x);
		_anim.SetFloat("Vertical", facePlayerDir.y);
    }

}