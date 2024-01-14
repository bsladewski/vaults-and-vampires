using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Dependencies")]
    [Required]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private CinemachineTransposer transposer;

    [Header("Follow Offsets")]
    [SerializeField]
    private Vector3 closeFollowOffset = new Vector3(0f, 1f, -6f);

    [SerializeField]
    private Vector3 normalFollowOffset = new Vector3(0f, 4f, -6f);

    [SerializeField]
    private Vector3 farFollowOffset = new Vector3(0f, 8f, -8f);

    [Header("Zoom Settings")]
    [SerializeField]
    private float zoomDuration = 1f;

    [SerializeField]
    private Ease zoomEase = Ease.OutBack;

    private enum FollowState
    {
        Close,
        Normal,
        Far
    }

    private FollowState followState;

    private Tween followOffsetTween;

    private void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        ApplyNormalFollowOffset();
    }

    public void EnableCamera()
    {
        virtualCamera.enabled = true;
    }

    public void DisableCamera()
    {
        virtualCamera.enabled = false;
    }

    public void ApplyCloseFollowOffset()
    {
        TweenFollowOffset(closeFollowOffset);
        followState = FollowState.Close;
    }

    public void ApplyNormalFollowOffset()
    {
        TweenFollowOffset(normalFollowOffset);
        followState = FollowState.Normal;
    }

    public void ApplyFarFollowOffset()
    {
        TweenFollowOffset(farFollowOffset);
        followState = FollowState.Far;
    }

    public void CycleFollowOffset()
    {
        switch (followState)
        {
            case FollowState.Normal:
                ApplyFarFollowOffset();
                break;
            case FollowState.Far:
                ApplyCloseFollowOffset();
                break;
            default:
                ApplyNormalFollowOffset();
                break;
        }
    }

    public void ResetCameraPosition(Vector3 position)
    {
        transposer.ForceCameraPosition(position + normalFollowOffset, Quaternion.identity);
    }

    private void TweenFollowOffset(Vector3 to)
    {
        if (followOffsetTween != null && followOffsetTween.active)
        {
            DOTween.Kill(followOffsetTween, true);
        }

        followOffsetTween = DOTween.To(
            () => transposer.m_FollowOffset,
            value => transposer.m_FollowOffset = value,
            to,
            zoomDuration
        ).SetEase(zoomEase);
    }
}
