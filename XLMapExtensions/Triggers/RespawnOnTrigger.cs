using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class RespawnOnTrigger : BoardTriggerBase
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            PlayerController.Instance.respawn.DoRespawn();
        }
    }
}
