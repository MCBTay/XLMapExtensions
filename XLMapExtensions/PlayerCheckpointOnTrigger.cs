using System;
using UnityEngine;

namespace XLMapExtensions
{
    [Serializable]
    public class PlayerCheckpointOnTrigger : MonoBehaviour
    {
        [Tooltip("The location for the player to teleport back to after a bail.")]
        public Transform CheckpointLocation;

        private Collider _boardCollider;

        private void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (CheckpointLocation == null) return;
            if (collider != _boardCollider) return;

            // Sets spawn position such that the next time the player bails or tries to go back to their marker, they go to the checkpoint
            PlayerController.Instance.respawn.SetSpawnPos(CheckpointLocation.position, CheckpointLocation.rotation);
        }
    }
}
