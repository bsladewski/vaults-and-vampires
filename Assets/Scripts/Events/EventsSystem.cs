using UnityEngine;

namespace Events
{
    public class EventsSystem : MonoBehaviour
    {
        public static EventsSystem Instance { get; private set; }

        public AbilityEvents abilityEvents { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Singleton instance EventSystem already instantiated!");
            }
            Instance = this;

            abilityEvents = new AbilityEvents();
        }
    }
}
