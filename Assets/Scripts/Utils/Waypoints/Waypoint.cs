using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    public Vector3 position;

    public Vector3 rotation;

    public float delay;

    public Ease ease = Ease.InOutQuad;
}
