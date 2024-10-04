using UnityEngine;

public class StartGameAfterIntroSequence : MonoBehaviour
{
    /// <summary>
    /// Called at the end of the Intro Sequence animation.
    /// </summary>
    public void TellGameManagerToStartGame()
    {
        GameManager.Instance.StartGame();
    }
}
