using UnityEngine;
using TMPro;

namespace Views
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private Transform collectPoint;
        [SerializeField] private string resourceName;

        public string ResourceName => resourceName;
        public Transform CollectPoint => collectPoint;

        // Можно использовать триггер, чтобы определить приближение игрока
        private void OnDrawGizmosSelected()
        {
            if (collectPoint)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(collectPoint.position, 0.3f);
            }
        }
    }
}