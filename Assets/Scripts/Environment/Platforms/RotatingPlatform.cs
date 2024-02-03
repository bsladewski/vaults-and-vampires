using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;

namespace Environment
{
    [SelectionBase]
    public class RotatingPlatform : MonoBehaviour, IMoverController
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Manages the movement of the kinematic rigidbody.")]
        [Required]
        [SerializeField]
        private PhysicsMover mover;

        [FoldoutGroup("Settings")]
        [Tooltip("The axis about which the platform will rotate.")]
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;

        [FoldoutGroup("Settings")]
        [Tooltip("Determines how quickly the platform rotates in degrees per second.")]
        [SerializeField]
        private float rotationSpeed = 60f;

        private void Awake()
        {
            mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = mover.transform.position;
            goalRotation = mover.transform.rotation;
            goalRotation *= Quaternion.Euler(rotationAxis * rotationSpeed * deltaTime);
        }
    }
}
