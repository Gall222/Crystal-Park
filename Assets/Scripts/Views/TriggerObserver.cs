using UniRx;
using UnityEngine;

namespace Views
{
    public class TriggerObserver : MonoBehaviour
    {
        public Subject<Collider> OnEnter = new();
        public Subject<Collider> OnExit = new();

        void OnTriggerEnter(Collider other) => OnEnter.OnNext(other);
        void OnTriggerExit(Collider other) => OnExit.OnNext(other);
    }
}