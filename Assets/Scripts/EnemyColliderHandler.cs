using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderHandler : MonoBehaviour
{
    [Header("Colliders")]
    public PolygonCollider2D defaultCollider;
    public List<PolygonCollider2D> frameColliders;

    private void Start()
    {
        EnableDefaultCollider();
    }

    /// <summary>
    /// Enables only the default collider.
    /// </summary>
    public void EnableDefaultCollider()
    {
        if (defaultCollider != null)
            defaultCollider.enabled = true;

        foreach (var col in frameColliders)
            if (col != null) col.enabled = false;
    }

    /// <summary>
    /// Enables a specific frame collider (called via animation event).
    /// </summary>
    /// <param name="index">Frame index</param>
    public void EnableFrameCollider(int index)
    {
        if (defaultCollider != null)
            defaultCollider.enabled = false;

        for (int i = 0; i < frameColliders.Count; i++)
            if (frameColliders[i] != null)
                frameColliders[i].enabled = (i == index);
    }
}
