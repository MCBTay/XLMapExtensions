﻿using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class TeleportPlayerOnTrigger : BoardTriggerBase
    {
        [Tooltip("The location to teleport the player to.")]
        public Transform LocationToTeleport;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (LocationToTeleport == null) return;
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            PlayerController.Instance.respawn.SetSpawnPos(LocationToTeleport.position, LocationToTeleport.rotation);
            PlayerController.Instance.respawn.DoRespawn();
        }
    }
}
