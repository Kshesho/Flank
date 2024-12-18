using UnityEngine;

/// <summary>
/// Destroys a Transform if it has no children.
/// </summary>
public class DestroyEmptyParent : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount < 1)
            Destroy(this.gameObject);
    }
}
