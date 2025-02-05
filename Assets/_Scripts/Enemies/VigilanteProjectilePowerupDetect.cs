#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;
#endregion

/// <summary>
/// Detects and collides with powerups
/// </summary>
public class VigilanteProjectilePowerupDetect : MonoBehaviour 
{
#region Variables

    [SerializeField] VigilanteKnife _knife;
    [SerializeField] Collider2D _thisCollider;

#endregion
#region Base Methods

    void Start () 
    {
		StartCoroutine(DisableMeRtn());
	}

#endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Powerup))
        {
            StartCoroutine(CollideWithPowerupRtn(other));
        }
    }
    IEnumerator CollideWithPowerupRtn(Collider2D other)
    {
        //turn off this collider so the VigilanteKnife doesn't use it to collide with the powerup too early
        _thisCollider.enabled = false;

        //Wait a frame before telling the knife it can collide with the powerup
        //this ensures the collider is fully disabled before the next physics update
        yield return new WaitForEndOfFrame();
        _knife.TargetPowerup(other.transform);
    }

    IEnumerator DisableMeRtn()
    {
        yield return HM.WaitTime(0.1f);
        this.gameObject.SetActive(false);
    }

}