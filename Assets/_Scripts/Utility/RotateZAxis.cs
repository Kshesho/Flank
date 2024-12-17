using UnityEngine;

/// <summary>
/// Rotates a gameObject continuously around its Z axis.
/// </summary>
public class RotateZAxis : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 1;

    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }

}
