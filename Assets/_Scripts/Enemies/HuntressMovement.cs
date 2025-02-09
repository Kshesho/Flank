using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement for the Huntress.
/// </summary>
public class HuntressMovement : EnemyMovement
{
#region Variables 

    [SerializeField] Rigidbody2D _rBody;
    bool _dodging;
    [SerializeField] float _dodgeForce = 200;
    [SerializeField] float _dodgeMoveSpeed = 2;

#endregion
#region Base Methods

    protected override void Start()
    {
        base.Start();
        MoveToSpawnPosition();
    }

    protected override void Update()
    {
        base.Update();
        DetectProjectile();
    }

#endregion

    void DetectProjectile()
    {
        RaycastHit2D hit;
        Vector2 direction = Vector2.down;
        float halfSize = 0.75f;
        
        //get the index of the layer to ignore
        int enemyLayer = LayerMask.NameToLayer("Ignore Raycast");
        //shift the 1 to the enemy layer's bit
        //invert all bits so that everything BUT the ignored layer is a 1
        int layerMask = ~(1 << enemyLayer);

        hit = Physics2D.BoxCast(transform.position, new Vector2(halfSize, halfSize), 0, direction, 1f, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(Tags.PProjectile))
                Dodge();
        }
    }

    void Dodge()
    {
        if (_dodging) return;
        StartCoroutine(DodgeCooldownRtn());

        //nudge my rigidbody towards 0 on the X
        Vector2 dir;
        if (transform.position.x < 0)
             dir = Vector2.right;
        else dir = Vector2.left;

        _rBody.AddForce(dir * _dodgeForce, ForceMode2D.Impulse);
    }
    /// <summary>
    /// This is used to prevent the raycast from triggering the dodge multiple times at once.
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCooldownRtn()
    {
        _dodging = true;
        _moveSpeed = _dodgeMoveSpeed;

        yield return HM.WaitTime(0.25f);

        _dodging = false;
        _moveSpeed = _baseMoveSpeed;
    }

}
