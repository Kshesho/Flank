#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

/// <summary>
/// Holds important global values and functions.
/// </summary>
public class GameManager : MonoSingleton<GameManager> 
{
#region Variables

    int _score;
    public int Score {  get { return _score; } }

    bool _gameOver;
    public bool GameOver { get { return _gameOver; } }

#endregion
#region Base Methods

    void OnEnable() 
    {
        Events.OnPlayerDeath += () => _gameOver = true;
	}
	
	void OnDisable() 
    {
		Events.OnPlayerDeath -= () => _gameOver = true;
	}

    void Update()
    {
        if (_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
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