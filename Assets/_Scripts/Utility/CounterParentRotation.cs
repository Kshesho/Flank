using UnityEngine;

/// <summary>
/// Sets this gameObject's Z rotation to the opposite of its parent, so that it doesn't rotate when the parent does.
/// </summary>
public class CounterParentRotation : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 
            transform.localRotation.eulerAngles.y, 
            -transform.parent.rotation.eulerAngles.z);
    }
}
