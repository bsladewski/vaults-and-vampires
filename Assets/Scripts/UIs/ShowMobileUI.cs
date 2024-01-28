using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UIs
{
    public class ShowMobileUI : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern bool IsMobile();

        [Header("Dependencies")]
        [Required]
        [SerializeField]
        private GameObject mobileControls;

        [Header("Settings")]
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
