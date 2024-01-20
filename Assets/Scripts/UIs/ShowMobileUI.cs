using Sirenix.OdinInspector;
using UnityEngine;

namespace UIs
{
    public class ShowMobileUI : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private GameObject mobileControls;

        [SerializeField]
        private bool showOnAllPlatforms;

        void Start()
        {
            if (showOnAllPlatforms || Application.isMobilePlatform)
            {
                mobileControls.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
