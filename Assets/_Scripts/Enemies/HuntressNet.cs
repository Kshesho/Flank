#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using UnityEngine;
#endregion

/// <summary>
/// Handle the Huntress' net movement and collision.
/// </summary>
public class HuntressNet : MonoBehaviour 
{
    #region Variables

    [SerializeField] CircleCollider2D _collider;
    [SerializeField] float _moveSpeed = 3;
    float _timeToStopAt;
    bool _moving = true;

    //Stop movement
    [SerializeField] SpriteRenderer _spriteRend;
    [SerializeField] RotateZAxis _rotateZAxis;
    Color _noAlpha = new Color(1, 1, 1, 0);
    [SerializeField] float _colorFadeSpeed = 0.1f;

#endregion
#region Base Methods

    void Start () 
    {
        transform.LookAt2D(GameManager.Instance.PlayerTransform());
        _timeToStopAt = Time.time + 0.8f;
	}
	
	void Update () 
    {
        if (_moving)
            transform.Translate(Vector2.up * _moveSpeed * Time.deltaTime, Space.Self);

        if (Time.time > _timeToStopAt && _moving)
        {
            StopMoving();
        }
	}

#endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            PlayerStateManager.Instance.NetStarted();
            Destroy(this.gameObject);
        }
    }

    void StopMoving()
    {
        _moving = false;
        transform.localScale *= 0.8f;
        _rotateZAxis.enabled = false;
        _collider.enabled = false;
        _spriteRend.sortingLayerName = "Foreground";
        _spriteRend.sortingOrder = -10;
        StartCoroutine(FadeAlphaRtn());
    }

    IEnumerator FadeAlphaRtn()
    {
        yield return HM.WaitTime(2.5f);

        while (_spriteRend.color.a > 0)
        {
            _spriteRend.color = Color.Lerp(_spriteRend.color, _noAlpha, Time.deltaTime * _colorFadeSpeed);
            yield return null;
        }
    }

}