#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

/// <summary>
/// (responsibility of this class)
/// </summary>
public class ChangeVolumeToggleSprite : MonoBehaviour 
{
#region Variables

    [SerializeField] Image _myImage;
    [SerializeField] Sprite _mutedSprite, _unMutedSprite;
    [SerializeField] bool _thisToggleIsForMusic;

#endregion

    void Start () 
    {
        ChangeSprite();
	}

    public void ChangeSprite()
    {
        bool muted;

        if (_thisToggleIsForMusic)
        {
            muted = AudioManager.Instance.MusicIsMuted;
        }
        else
        {
            muted = AudioManager.Instance.SfxIsMuted;
        }

        if (muted)
        {
            _myImage.sprite = _mutedSprite;
        }
        else
        {
            _myImage.sprite = _unMutedSprite;
        }
    }

}