using Sirenix.OdinInspector;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Required]
    [SerializeField]
    private Transform checkpointPositionTransform;

    public Vector3 GetCheckpointPosition()
    {
        return checkpointPositionTransform.position;
    }
}
