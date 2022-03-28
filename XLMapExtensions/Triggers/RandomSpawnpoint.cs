using System;
using System.Collections.Generic;
using UnityEngine;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class RandomSpawnpoint : MonoBehaviour
    {
        public GameObject spawnpoint;
        public List<Transform> spawnpointList = new List<Transform>();
        private int randomNumber;

        void Awake()
        {
            if (spawnpointList.Count == 0) return;
            
            randomNumber = UnityEngine.Random.Range(0, (spawnpointList.Count));
            spawnpoint.transform.position = spawnpointList[randomNumber].position;
        }
    }
}
