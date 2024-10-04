#region Using Statements
using UnityEngine;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class TreasureChest : MonoBehaviour 
{
#region Variables

    [SerializeField] Animator _anim;
    [SerializeField] Collider2D _thisCollider;

    [SerializeField] GameObject _introSequenceAnimation;

#endregion
#region Base Methods

    void OnEnable()
    {
        Events.OnCollide += OpenChest;
    }
    void OnDisable()
    {
        Events.OnCollide -= OpenChest;
    }

#endregion

    void OpenChest(Collider2D colliderBeingHit, int damage)
    {
        if (colliderBeingHit != _thisCollider)
            return;

        _anim.SetTrigger("open");
    }
    /// <summary>
    /// Called at the end of the Chest Open animation.
    /// </summary>
    public void StartIntroSequence()
    {
        _introSequenceAnimation.SetActive(true);
        this.gameObject.SetActive(false);
    }


}