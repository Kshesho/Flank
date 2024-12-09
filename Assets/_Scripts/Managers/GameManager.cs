#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

/// <summary>
/// Holds important global values, functions, and game state data.
/// </summary>
public class GameManager : MonoSingleton<GameManager> 
{
#region Variables

    int _score;
    public int Score {  get { return _score; } }

    bool _gameStarted;
    bool GameStarted { get { return _gameStarted; } }

    bool _gameOver;
    public bool GameOver { get { return _gameOver; } }

    bool _gamePaused;
    public bool GamePaused { get { return _gamePaused; } }

    public Transform PlayerTransform()
    {
        return GameObject.FindWithTag(Tags.Player).transform;
    }

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
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_gamePaused)
                    PauseGame();
                else ResumeGame();
                
            }
        }
    }

#endregion

    #region Game State

    public void StartGame()
    {
        _gameStarted = true;
        SpawnManager.Instance.StartSpawning();
        AudioManager.Instance.PlayMainTheme();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        UIManager.Instance.EnablePauseMenu();
        _gamePaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        UIManager.Instance.DisablePauseMenu();
        _gamePaused = false;
    }

    public void TERMINATE()
    {
        Application.Quit();
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