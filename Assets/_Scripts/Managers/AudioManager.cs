#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narzioth.Utilities;
#endregion

/// <summary>
/// Handles playing audio sources.
/// </summary>
public class AudioManager : MonoSingleton<AudioManager>
{
#region Variables

	[SerializeField] AudioSource _musicAuSource;

#endregion
#region Base Methods

    void Start () 
    {
		
	}
	
	void Update () 
    {
		
	}

#endregion

	public void PlayMainTheme()
	{
		_musicAuSource.Play();
	}


}