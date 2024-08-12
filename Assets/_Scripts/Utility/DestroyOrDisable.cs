#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Disables or destroys this utomatically or by an external trigger
/// </summary>
public class DestroyOrDisable : MonoBehaviour 
{
    #region Variables

    [Tooltip("True: destroys this gameObject. False: sets this gameObject inactive.")]
    [SerializeField] bool _destroy = true;

    [Tooltip("Disable this gameObject after a specified time")]
    [SerializeField] bool _timedDisable = true;
    [SerializeField] float _timeBeforeDisable = 10f;
    Coroutine _disableAfterDurationRtn;

    [SerializeField] bool _distanceDisable;
    Vector2 _startPos;
    [SerializeField] float _distanceBeforeDisable = 10f;

#endregion
#region Base Methods

    void Awake()
    {
        _startPos = transform.position;
    }

    void OnEnable() 
    {
        if (_timedDisable)
            _disableAfterDurationRtn = StartCoroutine(DisableAfterDurationRtn());
	}
	
	void Update () 
    {
        if (_distanceDisable)
            DisableAfterDisatnce();
        //  check distance from start
        //  disable if disableDistance met
	}

    private void OnDisable()
    {
        //make sure things get reset if necessary
        // call event?
    }

#endregion

    void Disable()
    {
        if (_destroy)
            Destroy(this.gameObject);
        else this.gameObject.SetActive(false);
    }

    IEnumerator DisableAfterDurationRtn()
    {
        yield return new WaitForSeconds(_timeBeforeDisable);
        Disable();
    }

    void DisableAfterDisatnce()
    {
        if (Vector2.Distance(transform.position, _startPos) > _distanceBeforeDisable)
        {
            Disable();
        }
    }

    //manual disable

    //cancel disable

}