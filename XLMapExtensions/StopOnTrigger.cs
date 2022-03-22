using System;
using System.Collections;
using UnityEngine;

namespace XLMapExtensions
{
    [Serializable]
    public class StopOnTrigger : MonoBehaviour
    {
        [Tooltip("The number of seconds to constrain the skater/board rigidbodies.")]
        public int TimeToFreeze;

        private Collider _boardCollider;
        private Rigidbody _skaterRigidbody;
        private Rigidbody _boardRigidbody;

        private void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];
            _skaterRigidbody = PlayerController.Instance.skaterController.skaterRigidbody;
            _boardRigidbody = PlayerController.Instance.boardController.boardRigidbody;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider != _boardCollider) return;

            _skaterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _boardRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private IEnumerator RemoveConstraints()
        {
            if (TimeToFreeze <= 0) TimeToFreeze = 5;

            yield return new WaitForSeconds(TimeToFreeze);

            _skaterRigidbody.constraints = RigidbodyConstraints.None;
            _boardRigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
