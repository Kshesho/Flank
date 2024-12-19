#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// Handle the Huntress' net behavior
/// </summary>
public class HuntressNet : MonoBehaviour 
{
#region Variables



#endregion
#region Base Methods

    void Awake()
    {
        
    }

    void Start () 
    {
		//face player
	}
	
	void Update () 
    {
		//move in direction facing
	}

#endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Events.OnPlayerNetted?.Invoke();
            Destroy(this.gameObject);
        }
    }

}