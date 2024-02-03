using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UIs
{
    public class ShowMobileUI : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern bool IsMobile();

        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("The object containing mobile control UI elements.")]
        [Required]
        [SerializeField]
        private GameObject mobileControls;

        [FoldoutGroup("Settings")]
        [Tooltip("Whether to show mobile controls on all platforms. This setting is useful for debugging mobile controls.")]
        [SerializeField]
        private bool showOnAllPlatforms;

        private void Start()
        {
            if (showOnAllPlatforms || CheckIsMobile())
            {
                mobileControls.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private bool CheckIsMobile()
        {
            if (Application.isMobilePlatform)
            {
                return true;
            }

#if !UNITY_EDITOR && UNITY_WEBGL
            return IsMobile();
#endif

            return false;
        }
    }
}
