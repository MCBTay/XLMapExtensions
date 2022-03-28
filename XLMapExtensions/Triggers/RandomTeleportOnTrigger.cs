using System;
using System.Collections.Generic;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class RandomTeleportOnTrigger : BoardTriggerBase
    {
        public List<Transform> tpLocations = new List<Transform>();
        private int randomNumber;

        void OnTriggerEnter(Collider collider)
        {
            if (collider != _boardCollider) return;
            if (!CanBeFiredAgain()) return;
            if (tpLocations.Count == 0) return;

            randomNumber = UnityEngine.Random.Range(0, tpLocations.Count);
            PlayerController.Instance.respawn.SetSpawnPos(tpLocations[randomNumber].position, tpLocations[randomNumber].rotation);
            PlayerController.Instance.respawn.DoRespawn();
        }
    }
}
