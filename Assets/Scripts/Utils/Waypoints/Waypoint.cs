using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [field: SerializeField]
    public Vector3 position { get; private set; }

    [field: SerializeField]
    public Vector3 rotation { get; private set; }

    [field: SerializeField]
    public float delay;

    [field: SerializeField]
    public Ease ease { get; private set; }

    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }

    public void SetRotation(Vector3 rotation)
    {
        this.rotation = rotation;
    }
}
