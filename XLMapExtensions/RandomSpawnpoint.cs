using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XLMapExtensions
{
    [Serializable]
    public class RandomSpawnpoint : MonoBehaviour
    {
        public GameObject spawnpoint;
        public List<Transform> spawnpointList = new List<Transform>();

        void Awake()
        {
            if (spawnpointList.Count == 0) return;
            
            var randomNumber = Random.Range(0, (spawnpointList.Count));
            spawnpoint.transform.position = spawnpointList[randomNumber].position;
        }
    }
}
