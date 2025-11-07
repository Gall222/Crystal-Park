using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public NavMeshAgent Agent { get; private set; }

        private readonly string _isMoving = "isMoving";
        private readonly string _isCollecting = "isCollecting";

        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (animator == null || Agent == null) return;

            animator.SetBool(_isMoving, Agent.velocity.magnitude > 0.1f && !Agent.pathPending);
        }

        public void PlayCollectingAnimation(bool isCollecting)
        {
            if (animator == null) return;
            animator.SetBool(_isCollecting, isCollecting);
        }
    }
}