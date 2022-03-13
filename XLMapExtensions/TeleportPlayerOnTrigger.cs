using System;
using UnityEngine;

namespace XLMapExtensions
{
    [Serializable]
    public class TeleportPlayerOnTrigger : MonoBehaviour
    {
        [Tooltip("The location to teleport the player to.")]
        public Transform LocationToTeleport;
        
        private Collider _boardCollider;

        private void Start()
        {
            _boardCollider = PlayerController.Instance.boardController.boardColliders[0];
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (LocationToTeleport == null) return;
            if (collider != _boardCollider) return;

            PlayerController.Instance.respawn.SetSpawnPos(LocationToTeleport.position, LocationToTeleport.rotation);
            PlayerController.Instance.respawn.DoRespawn();
        }
    }
}
