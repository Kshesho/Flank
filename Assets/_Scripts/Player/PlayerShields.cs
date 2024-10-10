#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles changing the visual state of the player's shields.
/// </summary>
public class PlayerShields : MonoBehaviour 
{
#region Variables

    [SerializeField] RotateZAxis _rotateZAxis;

    [SerializeField] GameObject _shield1, _shield2, _shield3;
    [SerializeField] SpriteRenderer _shieldSpriteRend1, _shieldSpriteRend2, _shieldSpriteRend3;

    [SerializeField] Sprite[] _shieldSprites;

#endregion

    public void AddAShield()
    {
        if (!_shield1.activeSelf)
        {
            _shieldSpriteRend1.sprite = RandomShieldSprite();
            _shield1.SetActive(true);
            ToggleRotatingShields(true);
            return;
        }
        if (!_shield2.activeSelf)
        {
            _shieldSpriteRend2.sprite = RandomShieldSprite();
            _shield2.SetActive(true);
            return;
        }
        if (!_shield3.activeSelf)
        {
            _shieldSpriteRend3.sprite = RandomShieldSprite();
            _shield3.SetActive(true);
        }
    }
    public void RemoveAShield()
    {
        if (_shield3.activeSelf)
        {
            _shield3.SetActive(false);
            return;
        }
        if (_shield2.activeSelf)
        {
            _shield2.SetActive(false);
            return;
        }
        if (_shield1.activeSelf)
        {
            _shield1.SetActive(false);
            ToggleRotatingShields(false);
        }
    }

    void ToggleRotatingShields(bool toggle)
    {
        _rotateZAxis.enabled = toggle;
    }

    Sprite RandomShieldSprite()
    {
        int rand = Random.Range(0, _shieldSprites.Length);
        return _shieldSprites[rand];
        // how do I make sure only unique sprites get picked? call recursively?
    }


}