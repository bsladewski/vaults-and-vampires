using UnityEngine;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Required]
    [SerializeField]
    private ThirdPersonCameraController thirdPersonCameraController;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton CameraManager already instantiated!");
        }
        Instance = this;
    }

    public ThirdPersonCameraController GetThirdPersonCameraController()
    {
        return thirdPersonCameraController;
    }
}
