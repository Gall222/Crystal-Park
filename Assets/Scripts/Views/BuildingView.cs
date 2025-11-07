using UnityEngine;

namespace Views
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private Transform collectPoint;
        [SerializeField] private string resourceName;

        public string ResourceName => resourceName;
        public Transform CollectPoint => collectPoint;
        
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