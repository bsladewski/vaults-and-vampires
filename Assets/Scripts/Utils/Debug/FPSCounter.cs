using UnityEngine;
using TMPro;

namespace Utils
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI counterText;

        [SerializeField]
        private float refreshRate = 1f;

        private float timer;

        private void Update()
        {
            if (Time.unscaledTime > timer)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                counterText.text = string.Format("FPS: {0}", fps);
                timer = Time.unscaledTime + refreshRate;
            }
        }
    }
}
