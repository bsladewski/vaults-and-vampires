using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class RandomClipPlayer : MonoBehaviour
    {
        [FoldoutGroup("Dependencies", expanded: true)]
        [Tooltip("Used to play audio clips.")]
        [Required]
        [SerializeField]
        protected AudioSource audioSource;

        protected void PlayRandomClip(AudioClip[] clips)
        {
            int index = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[index]);
        }
    }
}
