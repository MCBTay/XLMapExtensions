using System;
using UnityEngine;
using UnityModManagerNet;

namespace XLMapExtensions.Triggers
{
    [Serializable]
    public class RandomSpawnpoint : MonoBehaviour
    {
        private GameObject[] objects;
        private Transform spawnLocation;
        private int randomNumber;

        void Start()
        {
            objects = (GameObject.FindGameObjectsWithTag("SpawnPoint"));

            randomNumber = UnityEngine.Random.Range(0, (objects.Length - 1));
            spawnLocation = objects[randomNumber].transform;

/*            StartCoroutine(DelaySpawn(5f));

            for (int i = 0; i < objects.Length; i++)
            {
                UnityModManager.Logger.Log(objects[i].transform.position.ToString());
            }*/
        }

        void Update()
        {
            if(PlayerController.Instance.respawn.recentlyRespawned == true)
            {
                /*PlayerController.Instance.respawn.recentlyRespawned = false;*/
                UnityModManager.Logger.Log("recentlyRespawned == true");
                
                PlayerController.Instance.respawn.SetSpawnPos(spawnLocation.position, spawnLocation.rotation);
                PlayerController.Instance.respawn.DoRespawn();

                UnityModManager.Logger.Log(randomNumber.ToString());
                UnityModManager.Logger.Log(spawnLocation.position.ToString());
                gameObject.GetComponent<RandomSpawnpoint>().enabled = false;
            }
        }

/*        IEnumerator DelaySpawn(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            PlayerController.Instance.respawn.SetSpawnPos(spawnLocation.position, spawnLocation.rotation);
            PlayerController.Instance.respawn.DoRespawn();
            gameObject.GetComponent<RandomSpawnpoint>().enabled = false;
        }*/
    }
}
