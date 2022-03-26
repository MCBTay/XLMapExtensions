using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class BoardTriggerBase : MonoBehaviour
    {
        public float minTimeBetweenEvents;
        [Tooltip("Used only if minTimeBetweenEvents > 0.")]
        public bool useUnscaledTime;

        protected Collider _boardCollider;

        private float time => !useUnscaledTime ? Time.time : Time.unscaledTime;
        private float lastEventTime = float.MinValue;

        protected virtual void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];
        }

        /// <summary>
        /// A way to rate-limit trigger events from being fired.  If <see cref="minTimeBetweenEvents"/> was set to 0, this will always return true.  Else, it will check to see
        /// if enough time has elapsed (<see cref="minTimeBetweenEvents"/>) since the last firing and return whether or not it's capable to be fired again.
        /// </summary>
        /// <returns>True if enough time has elapsed since the last event was fired.  Returns false if not enough time has since the last event.</returns>
        protected bool CanBeFiredAgain()
        {
            if (minTimeBetweenEvents != 0 && time - minTimeBetweenEvents < lastEventTime)
                return false;

            lastEventTime = time;
            return true;
        }

        /// <summary>
        /// Used by editor to ensure <see cref="minTimeBetweenEvents" /> is never set negative.
        /// </summary>
        private void OnValidate()
        {
            if (minTimeBetweenEvents >= 0.0) return;

            minTimeBetweenEvents = 0.0f;
        }
    }
}
