using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerView : MonoBehaviour
    {
        private string _isMoving = "isMoving";
        private string _isCollecting = "isCollecting";
        private Animator _animator;
        public NavMeshAgent Agent { get; private set; }
        public bool IsCollecting { get; private set; }

        [Header("Rotation settings")]
        [SerializeField] private float rotationSpeed = 8f;

        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            Agent.updateRotation = false;
        }

        void Update()
        {
            if (Agent.isOnNavMesh)
            {
                bool isMoving = Agent.velocity.magnitude > 0.1f && !Agent.pathPending;

                _animator.SetBool(_isMoving, isMoving);
                
                if (isMoving)
                {
                    Vector3 dir = Agent.velocity.normalized;
                    if (dir.sqrMagnitude > 0.001f)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                    }
                }
            }
        }

        public void MoveTo(Vector3 point)
        {
            IsCollecting = false;
            _animator.SetBool(_isCollecting, false);
            
            if (!Agent.isOnNavMesh)
            {
                Debug.LogWarning("PlayerView.MoveTo(): Agent is not on NavMesh!");
                return;
            }

            Agent.isStopped = false;
            Agent.SetDestination(point);
        }

        public void StopAndCollect()
        {
            if (!Agent.isOnNavMesh) return;

            Agent.isStopped = true;
            IsCollecting = true;

            _animator.SetBool(_isMoving, false);
            _animator.SetBool(_isCollecting, true);
        }

        public void StopMoving()
        {
            if (!Agent.isOnNavMesh) return;

            Agent.isStopped = true;
            _animator.SetBool(_isMoving, false);
        }

        public bool HasReached(Vector3 target, float threshold = 0.5f)
        {
            if (Agent == null || !Agent.isOnNavMesh)
                return false;

            return !Agent.pathPending && Agent.remainingDistance <= threshold;
        }
    }
}
