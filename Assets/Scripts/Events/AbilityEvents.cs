using System;
using UnityEngine;
using Utils;

namespace Events
{
    public class AbilityEvents
    {
        public Action<GameObject, SpellType> OnSpellOrbTriggered;

        public Action<GameObject, SpellType> OnSpellUnlocked;
    }
}
