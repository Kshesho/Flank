using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement for the Huntress.
/// </summary>
public class HuntressMovement : EnemyMovement
{
    protected override void Start()
    {
        base.Start();
        MoveToSpawnPosition();
    }

}
