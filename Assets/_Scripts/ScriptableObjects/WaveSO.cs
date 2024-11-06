#region Using Statements
using UnityEngine;
#endregion

[CreateAssetMenu(fileName = "Wave.asset", menuName = "Enemy Wave")]
/// <summary>
/// Hold the data that constitutes a wave.
/// </summary>
public class WaveSO : ScriptableObject 
{
    public int enemies = 1;
    public float timeBetweenEnemies;

}