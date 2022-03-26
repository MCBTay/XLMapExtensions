using System;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class RespawnOnTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            PlayerController.Instance.respawn.DoRespawn();
        }
    }
}
