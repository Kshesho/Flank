#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Narzioth.Utilities;
#endregion

/// <summary>
/// Communicates with primary UI elements.
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
#region Variables

    [SerializeField] TextMeshProUGUI _scoreTxt;

#endregion
#region Base Methods

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

#endregion

    public void UpdateScoreText(int score)
    {
        _scoreTxt.text = "Kills: " + score;
    }


}