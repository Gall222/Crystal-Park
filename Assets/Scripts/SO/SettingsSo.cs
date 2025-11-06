using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class SettingsSo : ScriptableObject
    {
        [Header("Production")]
        public float productionInterval = 3f;
        public int productionPerCycle = 2;

        [Header("Collection")]
        public float resourceAddDelay = 0.2f;
        public float popupHideDelay = 1.5f;
        public int maxResourcePerType = 99;
        public int maxCapacity = 9999;

        [Header("UI")]
        public float popupSlideSpeed = 0.3f;

        [Header("Audio")]
        [Range(0f, 1f)] public float volume = 1f;
    }
}