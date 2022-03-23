using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class BoardTriggerBase : MonoBehaviour
    {
        protected Collider _boardCollider;

        protected virtual void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];
        }
    }
}
