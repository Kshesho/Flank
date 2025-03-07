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

    
    Transform _playerTransform;
    public Transform PlayerTransform()
    {
        Transform playerOrThisTransform = _playerTransform != null ? _playerTransform : this.transform;

        //If player is dead (game over) return this transform, which is off-screen
        if (GameOver)
            return this.transform;
        return playerOrThisTransform;
    }
    public Vector2 PlayerPosition()
    {
        var pos = PlayerTransform().position;
        //                        account for the player's center point being at their feet
        return new Vector2(pos.x, pos.y + 0.28f);
    }

#endregion
#region Base Methods

    private void Start()
    {
        _playerTransform = GameObject.FindWithTag(Tags.Player).transform;
    }
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

    public void BossKilled()
    {
        IncrementScore(20);
        //trigger UI
        //"Congratulations! You beat the game! But that was the easy part..."
        //a few seconds later
        //"Begin Endless Mode!"
        //  get rid of "Next Wave in" text
        //  spawn random enemy every # seconds.
        //  Every # seconds, increase the spawn rate
        //  player gets to see how high they can get their score
    }

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