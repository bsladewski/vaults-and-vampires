using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * 360f * Time.deltaTime, Space.World);
    }
}
