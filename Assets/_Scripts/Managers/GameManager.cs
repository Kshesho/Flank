#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Holds important global values and functions.
/// </summary>
public class GameManager : MonoSingleton<GameManager> 
{
#region Variables

    int _score;
    public int Score {  get { return _score; } }

#endregion
#region Base Methods

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

#endregion

    /// <summary>
    /// Adds 1 to the score, then updates the UI.
    /// </summary>
    public void IncrementScore()
    {
        _score++;
        UIManager.Instance.UpdateScoreText(_score);
    }
    public void IncrementScore(int amount)
    {
        _score += amount;
        UIManager.Instance.UpdateScoreText(_score);
    }

}