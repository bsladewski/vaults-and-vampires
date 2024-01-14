using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;

[SelectionBase]
public class RotatingPlatform : MonoBehaviour, IMoverController
{
    [Header("Dependencies")]
    [Required]
    [SerializeField]
    private PhysicsMover mover;

    [Header("Settings")]
    [SerializeField]
    private Vector3 rotationAxis = Vector3.up;

    [SerializeField]
    private float rotationSpeed;

    private void Awake()
    {
        mover.MoverController = this;
    }

    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = mover.transform.position;
        goalRotation = mover.transform.rotation;
        goalRotation *= Quaternion.Euler(rotationAxis * rotationSpeed);
    }
}
