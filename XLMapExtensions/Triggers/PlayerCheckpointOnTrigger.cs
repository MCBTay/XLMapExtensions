using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class PlayerCheckpointOnTrigger : BoardTriggerBase
    {
        [Tooltip("The location for the player to teleport back to after a bail.")]
        public Transform CheckpointLocation;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (CheckpointLocation == null) return;
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;

            // Sets spawn position such that the next time the player bails or tries to go back to their marker, they go to the checkpoint
            PlayerController.Instance.respawn.SetSpawnPos(CheckpointLocation.position, CheckpointLocation.rotation);
        }
    }
}
