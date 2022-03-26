using System;
using System.Collections;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class StopOnTrigger : BoardTriggerBase
    {
        [Tooltip("The number of seconds to constrain the skater/board rigidbodies.")]
        public int TimeToFreeze;

        private Rigidbody _skaterRigidbody;
        private Rigidbody _boardRigidbody;

        protected override void Start()
        {
            base.Start();

            _skaterRigidbody = PlayerController.Instance.skaterController.skaterRigidbody;
            _boardRigidbody = PlayerController.Instance.boardController.boardRigidbody;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            _skaterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _boardRigidbody.constraints = RigidbodyConstraints.FreezeAll;

            StartCoroutine(RemoveConstraints());
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
