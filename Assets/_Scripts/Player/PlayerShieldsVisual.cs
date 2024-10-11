#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handles changing the visual state of the player's shields.
/// </summary>
public class PlayerShieldsVisual : MonoBehaviour 
{
#region Variables

    [SerializeField] RotateZAxis _rotateZAxis;

    [SerializeField] GameObject _shield1, _shield2, _shield3;
    [SerializeField] SpriteRenderer _shieldSpriteRend1, _shieldSpriteRend2, _shieldSpriteRend3;

    [SerializeField] Sprite[] _shieldSprites;
    List<Sprite> _activeShieldSprites = new List<Sprite>();

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
            RemoveActiveShieldSprite(_shieldSpriteRend3);
            return;
        }
        if (_shield2.activeSelf)
        {
            _shield2.SetActive(false);
            RemoveActiveShieldSprite(_shieldSpriteRend2);
            return;
        }
        if (_shield1.activeSelf)
        {
            _shield1.SetActive(false);
            RemoveActiveShieldSprite(_shieldSpriteRend1);
            ToggleRotatingShields(false);
        }
    }

    /// <summary>
    /// Returns a random shield sprite and adds it to the active shield sprites list.
    /// </summary>
    /// <returns></returns>
    Sprite RandomShieldSprite()
    {
        int rand = Random.Range(0, _shieldSprites.Length);
        Sprite chosenSprite = _shieldSprites[rand];

        while (_activeShieldSprites.Contains(chosenSprite))
        {
            rand = Random.Range(0, _shieldSprites.Length);
            chosenSprite = _shieldSprites[rand];
        }

        _activeShieldSprites.Add(chosenSprite);
        return chosenSprite;
    }

    void RemoveActiveShieldSprite(SpriteRenderer shieldSpriteRend)
    {
        _activeShieldSprites.Remove(shieldSpriteRend.sprite);
    }

    void ToggleRotatingShields(bool toggle)
    {
        _rotateZAxis.enabled = toggle;
    }


}