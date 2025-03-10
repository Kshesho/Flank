using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameSequenceKeyframeEvents : MonoBehaviour
{
    /// <summary>
    /// Called via keyframe event on "Endgame Sequence" animation
    /// </summary>
    public void EndgameSequenceAnimFinished()
    {
        GameManager.Instance.BeginEndgame();
    }

}
